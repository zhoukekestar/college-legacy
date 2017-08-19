#include <stdio.h>
#include <string.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <iostream>
#include <sstream>

#include <errno.h>
#include <fcntl.h>
#include <stdio.h>
#include <string.h>
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <iostream>

using namespace std;

#ifndef MOD_SOCKETCLIENT
#define MOD_SOCKETCLIENT

class SocketClient
{
private:
    int s;
    struct sockaddr_in addr;
    
    char recv_buffer[256];
    int recv_len;
    
    uint16_t port;
    const char* server_ip;
	string sendFile;
    string recvFile;
	
    void init()
    {
		cout << "Client is try " << server_ip << ":" << port << endl;
		//cout << port << " " << server_ip << endl;
        if((s = socket(AF_INET,SOCK_STREAM,0))<0)
        {
			cout << " socket error" << endl;
            perror("socket");
            return;
        }
        bzero(&addr,sizeof(addr));
        addr.sin_family = AF_INET;
        addr.sin_port=htons(port);
        addr.sin_addr.s_addr = inet_addr(server_ip);
        /* 尝试连线*/
        if(connect(s,(sockaddr*)&addr,sizeof(addr))<0)
        {
			cout << "connect error" << endl;
            perror("connect");
            return;
        }
    }
public:
    SocketClient()
    {
		port = 1234;
		server_ip = (char *)"127.0.0.1";
		sendFile = "send";
		recvFile = "recv";
		
        init();
    }
	SocketClient(uint16_t _port, const char* _server_ip)
    {
        port = _port;
        server_ip = _server_ip;
		sendFile = "send";
		recvFile = "recv";
		
        init();
    }
    SocketClient(uint16_t _port, char* _server_ip, string send, string recv)
    {
        port = _port;
        server_ip = _server_ip;
		sendFile = send;
		recvFile = recv;
		
        init();
    }
	string getSendFile()
	{
		return sendFile;
	}
	string getRecvFile()
	{
		return recvFile;
	}
	
    char* recvMsg()
    {
		bzero(&recv_buffer,sizeof(recv_buffer));
        recv(s,recv_buffer, sizeof(recv_buffer), 0);
        recv_len = sizeof(recv_buffer);
        return recv_buffer;
    }
    int getRecvLen()
    {
		return recv_len;
	}
    bool sendMsg(char* msg, int len)
    {
        if(send(s,msg,len,0)<0)
        {
            perror("send");
            return false;
        }
        return true;
    }
    
    static void test()
    {
    	SocketClient client(1234, (char*)"10.64.130.222");
    	string msg = "hello this is client.";
    	client.sendMsg((char*)msg.c_str(), msg.length());
    	string recv = client.recvMsg();
    	cout << "recv[" << recv << "]" << endl;
    }
};

#endif
