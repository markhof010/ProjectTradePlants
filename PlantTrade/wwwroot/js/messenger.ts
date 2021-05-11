// Globals
const sendButton: HTMLElement = document.querySelector('.messenger .msg-send');
const msgInput: HTMLInputElement = document.querySelector('.messenger .msg-chatbox');
let msgDummy: HTMLElement = null;
let maxContainerWidth: number = null;
let inputLineHeight: number = msgInput.clientHeight;
let conversation: Conversation = null;
let cache = {};
let que = [];
let ignoreSent: number = 0;

// Classes
class Conversation {
    private _id: string;
    private _userId: string;
    private _recieverId: string;
    private _senderId: string;

    constructor(id: number, recieverId: string, senderId: string, userId?: string) {
        this._id = id.toString();
        this._recieverId = recieverId;
        this._senderId = senderId;
        this._userId = userId ? userId : '';
    }

    get id(): string {
        return this._id;
    }

    get recieverId(): string {
        return this._recieverId;
    }

    get senderId(): string {
        return this._senderId;
    }

    get userId(): string {
        return this._userId;
    }
}

class DBMessage {
    public Id: number;
    public Message1: string;
    public UserId: string;
    public ConversationId: string;
    public TimeStamp: Date;

    constructor(text: string, userId: string, conversationId: string) {
        this.Message1 = text;
        this.UserId = userId;
        this.ConversationId = conversationId;
    }

    public static convert(message: Message): DBMessage {
        return new DBMessage(message.text, message.isSender ? conversation.senderId : conversation.recieverId, conversation.id);
    }
}

class Message {
    public text: string;
    public date: Date;
    public isSender: false;

    constructor(text, isSender, date?) {
        this.text = text;
        this.date = date ? date : new Date();
        this.isSender = isSender;
    }
}


// Functions
function request(
    method: 'GET' | 'POST',
    url: string,
    content?: DBMessage,
    callback?: (response: Response) => void,
    errorCallback?: (error: any) => void,
    sync: boolean = true)
{
    const req: XMLHttpRequest = new XMLHttpRequest();

    req.open(method, url, sync);
    req.onload = (ev) => {
        console.log('Loaded');
        console.log(ev.currentTarget);
        if (this.status == 200) {
            const data: Response = <Response>JSON.parse(this.response);

            callback(data);
        }
        else {
            console.log('Something went wrong with a request!');
            errorCallback(this);
        }
    }

    if (method == 'POST') {
        req.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
    }

    console.log('Sending object:', method, content);
    req.send(JSON.stringify(content));
}

function sendMessage(text): void {
    if (text.length > 0) {
        const msg: Message = new Message(text, true);

        msgInput.value = '';
        createMessageEl(msg);
        msgDummy.innerText = '';

        fetch('/Messages/CreateMessage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Message1: msg.text,
                UserId: conversation.userId,
                ConversationId: parseInt(conversation.id)
            }) //DBMessage.convert(msg)
        }).then(res => {
            if (!res.ok) {
                console.log('Error:', res);
            }
            else {
                ignoreSent++;
            }
        }).catch(error => {
            console.log('Error posting:', error);
        });

        //request('POST', '/Messages/CreateMessage', DBMessage.convert(msg));
    }
}

function createMessageEl(msg: Message): void {
    let elMsgContainer: HTMLElement;
    const elMsgBody: HTMLElement = document.querySelector('.messenger .msg-body');
    const elMsgBubble: HTMLElement = document.createElement('div');
    const text: Node = document.createTextNode(msg.text);

    if (!elMsgBody.querySelectorAll('.msg-bubble').length ||
        !elMsgBody.querySelector('.msg-container:last-child').classList.contains(msg.isSender ? 'sender' : 'reciever'))
    {
        elMsgContainer = document.createElement('div');
        elMsgContainer.classList.add('msg-container');
        elMsgContainer.classList.add(msg.isSender ? 'sender' : 'reciever');
        elMsgBody.appendChild(elMsgContainer);

        const triangle: HTMLElement = document.createElement('div');
        triangle.classList.add('triangle')
        elMsgBubble.appendChild(triangle);
    }
    else {
        elMsgContainer = document.querySelector('.msg-container:last-child');
        const prevBubble: HTMLElement = elMsgContainer.querySelector('.msg-bubble:last-child');

        if (msgDummy.offsetWidth > prevBubble.offsetWidth) {
            elMsgBubble.classList.add('top-' + (msg.isSender ? 'left' : 'right' ) + '-radius');
        }
        else if (msgDummy.offsetWidth < prevBubble.offsetWidth) {
            prevBubble.classList.add('bot-' + (msg.isSender ? 'left' : 'right') + '-radius');
        }
    }

    elMsgBubble.classList.add('msg-bubble', 'enlarge');
    msg.isSender ? elMsgBubble.classList.add('sender') : elMsgBubble.classList.add('reciever');
    elMsgBubble.appendChild(text);
    elMsgContainer.appendChild(elMsgBubble);

    elMsgBody.scrollTop = elMsgBody.scrollHeight;
    elMsgBubble.classList.remove('enlarge');
}

function loadMessenger(data: object): void {
    sendButton.addEventListener('click', () => {
        sendMessage(msgInput.value);
    });

    msgInput.addEventListener('keypress', (event: KeyboardEvent) => {
        if (event.keyCode == 13) {
            event.preventDefault();
            sendMessage(msgInput.value);
        }

        if (event.keyCode == 96) {
            let simulate: HTMLInputElement = document.querySelector('.messenger .simulate');
            simulate.checked = !simulate.checked;
            window.setTimeout(() => { msgInput.value = ''; msgDummy.innerText = ''; }, 0);
        }
    });

    console.log('Messenger loaded succesfully!');

    window.setTimeout(() => {
        const elMsgBody: HTMLElement = document.querySelector('.messenger .msg-body');
        elMsgBody.scrollTop = elMsgBody.scrollHeight;

        document.querySelector('.messenger').classList.remove('init');
    }, 0);

    window.setTimeout(() => {
        fetchNewMessages();
    }, 1000);
}

function loadMessages(data: Array<DBMessage>): void {
    for (let i = data.length - 1; i >= 0; i--) {
        createMessageEl(new Message(data[i].Message1, data[i].UserId == conversation.userId, data[i].TimeStamp));
        cache[data[i].Id.toString()] = true;
    }

    loadMessenger(data);
}

function fetchMessages(data: object): void {
    if (data['TraderName'] != 'INVALID') {
        conversation = new Conversation(data['ConversationId'], data['TraderId'], data['SenderId'], data['UserId']);

        (<HTMLElement>document.querySelector('h3.reciever-name')).innerText = data['TraderName'];

        fetch('/Messages/GetRecentMessages/' + conversation.id, { method: 'GET' }).then(res => {
            if (!res.ok) {
                console.log('Error; could not fetch!');
            }

            res.json().then(data => loadMessages(data));
        }).catch(error => {
            console.log('Error:', error);
        });
    }
    else {
        const elMessenger: HTMLElement = document.querySelector('.messenger');
        const elCC: HTMLElement = document.querySelector('.create-convo');

        elMessenger.classList.add('hidden');
        elCC.classList.remove('hidden');
    }
}

function fetchNewMessages(): void {
    fetch('/Messages/GetRecentMessages/' + conversation.id, { method: 'GET' }).then(res => {
        if (!res.ok) {
            console.log('Error; could not fetch new messages!');
        }

        res.json().then(data => {
            for (let i = 0; i < data.length; i++) {
                if (cache[data[i].Id.toString()]) {
                    const elMsgBody: HTMLElement = document.querySelector('.messenger .msg-body');
                    window.setTimeout(() => fetchNewMessages(), 3000);

                    for (let i = que.length - 1; i >= 0; i--) {
                        createMessageEl(new Message(que[i].Message1, que[i].UserId == conversation.userId, que[i].TimeStamp));
                    }

                    if (que.length) {
                        window.setTimeout(() => elMsgBody.scrollTop = elMsgBody.scrollHeight, 300);
                    }

                    que = [];
                    return;
                }
                else if (data[i].UserId == conversation.senderId && ignoreSent > 0) {
                    cache[data[i].Id.toString()] = true;
                    ignoreSent--;
                }
                else {
                    que.push(data[i]);
                    cache[data[i].Id.toString()] = true;
                }
            }
        });
    }).catch(error => {
        console.log('Error:', error);
    });
}

// Events
msgInput.addEventListener('input', () => {
    msgDummy.innerText = msgInput.value;

    //if (msgDummy.offsetWidth >= msgInput.offsetWidth) {
    //    msgInput.style.height = (msgInput.clientHeight + inputLineHeight) + 'px';
    //    msgInput.style.bottom = '-20px';
    //}
});

// On run
window.addEventListener('load', () => {
    const elMessenger: HTMLElement = document.querySelector('.messenger');

    msgDummy = document.createElement('div');
    msgDummy.classList.add('msg-dummy', 'msg-bubble');
    elMessenger.appendChild(msgDummy);

    maxContainerWidth = (<HTMLElement>document.querySelector('.messenger .msg-body')).offsetWidth - 50;

    let prefix: string = '/Messages/Index/';

    let newStr: string = window.location.pathname.replace(prefix, '');

    if (newStr == window.location.pathname) {
        prefix = '/Items/ViewItem/';
        newStr = window.location.pathname.replace(prefix, '');

        console.log(newStr);

        fetch('/Messages/GetConversationWithItem/' + newStr, { method: 'GET' }).then(res => {
            if (!res.ok) {
                console.log('Error; could not fetch! withItem');
            }

            res.json().then(data => { fetchMessages(data) });
        }).catch(error => {
            console.log('Error:', error);
        });
    }
    else {

        fetch('/Messages/GetConversation/' + newStr, { method: 'GET' }).then(res => {
            if (!res.ok) {
                console.log('Error; could not fetch!');
            }

            res.json().then(data => { fetchMessages(data) });
        }).catch(error => {
            console.log('Error:', error);
        });
    }
});