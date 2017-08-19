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

#include <pthread.h>
#include <list>

using namespace std;

#ifndef MOD_THREADPOOL
#define MOD_THREADPOOL

class ThreadPool
{
private:
	list<pthread_t> threadList;
public:	
	void addMethod(void* (*func)(void *arg), void* args)
	{
		pthread_t temp;
		if (pthread_create(&temp, NULL, func, args) != 0)
		{
			cout << "Create thread error!" << endl;
			return;
		}
		threadList.push_back(temp);
	}
	
	void wait()
	{
		list<pthread_t>::iterator it;
		for (it = threadList.begin(); it != threadList.end(); it++)
		{
			pthread_join(*it, NULL);
		}
	}
	
	void killAll()
	{
		list<pthread_t>::iterator it;
		for (it = threadList.begin(); it != threadList.end(); it++)
		{
			pthread_cancel(*it);
		}
	}
	
	static void* testFunc(void*arg)
	{
		for (int i = 0; i < 4; i++)
		{
			cout << (char*)arg << endl;
			sleep(1);
		}
		return (void*)0;
	}
	
	static void test()
	{
		ThreadPool pool;
		pool.addMethod(ThreadPool::testFunc, (void*)"hello");
		pool.addMethod(ThreadPool::testFunc, (void*)"hi");
		pool.wait();
	}
};

#endif
