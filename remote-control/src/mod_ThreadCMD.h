#include "mod_SysCMD.h"

using namespace std;

#ifndef MOD_THREADCMD
#define MOD_THREADCMD

class ThreadCMD
{
private:
public:
	static void* threadRun(void* arg)
	{
		string str = (char*)arg;
		SysCMD cmd(str);
		while(1)
		{
			string msg = cmd.get();
			cmd.del();
			
			msg = SysCMD::convertOneLine(msg);
			if (msg.length() != 0)
			{
				cout << "msg:[" << msg << "]" << endl;
				SysCMD::exe(msg);
			}
			else
			{
				sleep(1);
			}
			sleep(3);
		}
		return (void*)0;
	}
};

#endif