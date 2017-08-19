#include "mod_SocketClient.h"
#include "mod_SysCMD.h"
#include <iostream>
#include <string>

using namespace std;

#ifndef MOD_THREADSOCKETCLIENT
#define MOD_THREADSOCKETCLIENT

class ThreadSocketClient
{
private:

public:

	static void* threadRecv(void* arg)
	{
		SocketClient* client = (SocketClient*)arg;
		SysCMD recv((*client).getRecvFile());
		while(1)
		{
			string msg = (*client).recvMsg();
			recv.add(msg);
			cout << "recv:[" << msg << "]" << endl;
			sleep(1);
		}
		return (void*)0;
	}
	
	static void* threadSend(void* arg)
	{
		SocketClient* client = (SocketClient*)arg;
		SysCMD send((*client).getSendFile());
		while(1)
		{
			string msg = send.get();
			send.del();
			if (msg.length() != 0)
			{
				(*client).sendMsg((char*)msg.c_str(), msg.length());
				msg = SysCMD::convertOneLine(msg);
				cout << "send:[" << msg << "]" << endl;
			}
			else
			{
				sleep(1);
			}
			sleep(1);
		}
		return (void*)0;
	}
};

#endif
