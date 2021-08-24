# web-socket-playground

.NET 4.5.1 project working with web sockets.

Library used for web sockets is [websocket-sharp](https://github.com/sta/websocket-sharp "websocket-sharp").

Starting server from `Server` project, nothing else is needed on server side.

Starting client from `Playground` project. 
Playground project is starting web socket client and periodically pinging server.
If server doesn't respong with a pong message, recovery process will be started.
Attempts are hardcoded at 3, but can be changed if needed.
There are some edge cases that aren't covered, this was simply done for testing purposes. 
