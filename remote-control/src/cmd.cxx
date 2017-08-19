#include "mod_SysCMD.h"
#include "mod_ThreadPool.h"
#include "mod_ThreadCMD.h"

using namespace std;

int main(int argn, char** argv)
{
	string filename = "recv";
	if (argn < 2)
	{
		cout << "Listen default file: recv." << endl;
	}
	else if (argn == 2)
	{
		filename = argv[1];
		cout << "Listen file:" << filename << ".\n";
	}
	else
	{
		cout << "WARN:Program accept one param, others will be ignored." << endl;
		filename = argv[1];
		cout << "Listen file:" << filename << ".\n";
	}
	ThreadPool pool;
	pool.addMethod(ThreadCMD::threadRun, (void*)filename.c_str());
	pool.wait();
	return 0;
}
