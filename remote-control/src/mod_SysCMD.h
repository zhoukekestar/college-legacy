#include <stdio.h>
#include <iostream>
#include <fstream>
#include <list>

#ifndef MOD_SYSCMD
#define MOD_SYSCMD

using namespace std;

class SysCMD
{
private:
	static string convertColon(string str)
	{
		unsigned int i = 0;
		for (i = 0; i < str.length(); i++)
		{
			if (str[i] == '"')
			{
				if (i != 0 && str[i - 1] == '\\')
					continue;
				str.replace(i, 1, "\\\"");
				i++;
			}
		}
		return str;
	}
	
	
	
	string file;
public:	
	SysCMD()
	{
		file = "temp";
	}
	SysCMD(string f)
	{
		file = f;
	}
	static string convertOneLine(string str)
	{
		unsigned int i = 0;
		for (i = 0; i < str.length(); i++)
		{
			if (str[i] == '\n')
			{
				str[i] = ' ';
			}
			else if (str[i] == '\r')
			{
				str[i] = ' ';
			}
		}
		return str;
	}
	static list<string> exe(string cmd)
	{
		cmd += " 2>/dev/null";
		cmd = SysCMD::convertOneLine(cmd);
		
		list<string> result;
		
		FILE* fp;
		fp = popen(cmd.c_str(), "r");
		
		char buffer[800];
		while (fgets(buffer, sizeof(buffer), fp) != NULL)
		{
			result.push_back(buffer);
		}
	
		pclose(fp);
		if (result.size() == 0)
			result.push_back("");
		return result;
		
	}
	
	void add(string msg)
	{
		SysCMD::addTo(msg, file);
	}
	
	void del()
	{
		SysCMD::delFirstLine(file);
	}
	
	string get()
	{
		string str = SysCMD::getFrom(file);
		if (str.length() <= 1)
			str = "";
		return str;
	}
	
	static void addTo(string msg, string file)
	{
		string cmd = "echo \"" + SysCMD::convertColon(msg) + "\" >> " + file;
		SysCMD::exe(cmd);
	}
	
	static string getFrom(string file)
	{
		string cmd = "sed -n '1p' " + file;
		return (SysCMD::exe(cmd)).front();
	}
	
	static void delFirstLine(string file)
	{
		string cmd = "sed -i '1d' " + file;
		SysCMD::exe(cmd);
	}
	
	static void showList(list<string> li)
	{
		cout << "showList---" << endl;
		list<string>::iterator it = li.begin();
		for (; it != li.end(); it++)
		{
			cout << *it << endl;
		}
		cout << "showList---" << endl;
	}
	
	static void test()
	{
		SysCMD cmd;
		cmd.add("1");
		cmd.add("2");
		cmd.add("3");
		cout << cmd.get() << endl;
		cmd.del();
		cout << cmd.get() << endl;
	}
};

#endif
