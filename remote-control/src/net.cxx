#include "mod_SysCMD.h"
#include "mod_ThreadPool.h"
#include "mod_ThreadSocketClient.h"
#include <stdio.h>

using namespace std;

void modTest()
{
	SysCMD::test();
	ThreadPool::test();
	SocketClient::test();
}

int main(int argn, char** argv)
{
	int port = 1234;
	string ip;
	
	if (argn != 3)
	{
		cout << "Please input the ip & port. \nEXAMPLE:\n" << argv[0] << " 127.0.0.1 1234\n" << endl;
		return 0;
	}
	else
	{
		sscanf(argv[2], "%d", &port);
		ip = argv[1];
		cout << "Listen to " << ip << ":" << port << endl;
	}
	ThreadPool pool;
	SocketClient client(port, ip.c_str());

	pool.addMethod(ThreadSocketClient::threadSend, (void*)&client);
	pool.addMethod(ThreadSocketClient::threadRecv, (void*)&client);
	pool.wait();
	return 0;
}
