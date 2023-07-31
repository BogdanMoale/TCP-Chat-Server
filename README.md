# TCP-Chat-Server

C# Windows Forms application for a simple chat server. The application is made form two parts: the server side and the client side. The server listens for incoming client connections and allows clients to communicate with each other. Let's go through the important parts of the code and explain its functionality:

# Server

1. Main Class and Form Initialization:
The Main class is a Windows Forms application form. It contains a constructor that initializes the form, sets up event handlers, and creates a Listener object to listen for incoming connections on port 2014.

2. List of Clients:
The clients list is used to store references to all connected clients. Each client is represented by a Socket object.

3. BroadcastData Method:
The BroadcastData method sends a message to all connected clients. It iterates through each client in the clients list and sends the provided data (in ASCII encoding) to each of them.

4. Listener_SocketAccepted Method:
When a new client connection is accepted by the server, this event handler method is called. It creates a Client object to handle the newly connected client. The Client class is not shown in the provided code, but it is assumed to handle individual client interactions.

5. Client_Disconnected Method:
When a client disconnects, this event handler method is called. It removes the client from the list, updates the UI to reflect the disconnection, and sends a message to all remaining clients to refresh their chat UI.

6. Client_Received Method:
This event handler method is called when the server receives data from a client. It processes the data based on the command sent by the client. The commands are split using the '|' character, and different actions are taken based on the command type.

7. OnFormClosing Method:
This method is overridden to ensure that the server's Listener is stopped when the form is closing.

8. btnSend_Click Method:
When the "Send" button is clicked, this method sends the input message to all connected clients using the BroadcastData method. It also updates the chat UI for the server.

9. disconnectToolStripMenuItem_Click Method:
This method is called when the "Disconnect" menu item is clicked. It sends a "Disconnect" command to the selected clients to request them to disconnect.

10. chatWithClientToolStripMenuItem_Click Method:
This method is called when the "Chat" menu item is clicked. It sends a "Chat" command to the selected clients to initiate a private chat with each selected client.

11. txtInput_KeyDown Method:
This method handles the "Enter" key press event in the input text box. When the "Enter" key is pressed, it triggers the "Send" button click event.

12. txtReceive_TextChanged Method:
This method ensures that the text box scrolls to the end whenever the text changes, providing a smooth chat experience.

Listener class and the Client class is handling the low-level networking interactions, such as listening for incoming connections, sending and receiving data, and managing client sockets.

# Client

Windows Forms application for the public chat window where clients can communicate with each other in a chat room. It uses a LoginForm to handle the initial login and connection to the chat server. Let's go through the key parts of the PublicChatForm class:

1. Fields and Properties:

* formLogin: This field holds an instance of the LoginForm, which is used for handling the initial login and client connection to the server.
* pChat: This field holds an instance of the PrivateChatForm, which is used for private one-on-one chats between clients.

2. Constructor:
The constructor initializes the PublicChatForm object. It creates a new instance of the LoginForm and a new instance of the PrivateChatForm.

3. OnLoad Method:
This method overrides the base OnLoad method and is called when the public chat form is loaded. It subscribes to the Received event and the Disconnected event of the ClientSettings object (obtained from the LoginForm). The Received event is triggered when the client receives data from the server, and the Disconnected event is triggered when the client is disconnected from the server.

The method sets the title of the public chat form to display the server IP address and the nickname of the connected client. It then shows the login form as a modal dialog using formLogin.ShowDialog(). This ensures that the user must log in and connect to the server before proceeding to the chat window.

4. Client_Disconnected Method:
This method is not implemented in the provided code snippet. It would handle actions when the client is disconnected from the server. Currently, it is empty.

5. _client_Received Method:
This method is the event handler for the Received event of the ClientSettings object. It processes the received data from the server based on the command sent by the server. The data is split using the '|' character, and different actions are taken based on the command type.

* If the command is "Users," the method updates the user list in the chat window with the names of connected users (excluding "Connected" and "RefreshChat" status).
* If the command is "Message," the method displays the received message in the public chat window.
* If the command is "RefreshChat," the method refreshes the entire chat history in the public chat window.
* If the command is "Chat," the method opens the private chat window (pChat) and updates its title with the user's nickname.
* If the command is "pMessage," the method displays a private message received from the server in the private chat window (pChat).
* If the command is "Disconnect," the method closes the entire application using Application.Exit().

6. btnSend_Click Method:
This method is called when the "Send" button is clicked in the public chat window. It sends the input message to the server using the Send method of the ClientSettings object obtained from formLogin. The message includes the command "Message," the client's nickname, and the message content. It then updates the public chat window with the sent message.

7. txtInput_KeyDown Method:
This method handles the "Enter" key press event in the input text box. When the "Enter" key is pressed, it triggers the "Send" button click event (by calling btnSend.PerformClick()).

8. txtReceive_TextChanged Method:
This method ensures that the text box scrolls to the end whenever the text changes, providing a smooth chat experience.

9. privateChat_Click Method:
This method is called when the "privateChat" button (presumably a button to initiate a private chat) is clicked. It sends a "pChat" command to the server along with the client's nickname, indicating that the client wants to initiate a private chat. It then shows the private chat window (pChat) and updates its title with the user's nickname.