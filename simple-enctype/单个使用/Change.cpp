#include <iostream>
#include <fstream>
#include <algorithm>

#include <string>
#include <time.h>

using namespace std;

class MyFileChangeClass
{
	int midu;
public:
	MyFileChangeClass(string _name)
	{
		size = -1;
		midu = 1024;
		name = _name;
		MyInit();
		if (size != -1)
			delete[] buffer;
	}	
private:	
	string name;
	string instr, outstr;
	
	char * buffer;
	long size;

	
	void ChangeFun()
	{
		string str = "CH_";
		str += name;
		str += "temp";
		FILE *change = fopen(str.c_str(), "w");

		srand(0);
		if (change != 0)
		{
			for (int i = 0; i < 1024; i++)
			{
				if (rand() % 2 == 0)
				{
					fprintf(change, "%d %d\n", i, int(buffer[i]));
					buffer[i] = (char)rand() % 128;
				}
			}
			
			for (int i = 1024; i < size; i+=(rand() % midu))
			{	
				fprintf(change, "%d %d\n", i, (int)(buffer[i]));
				buffer[i] = (char)rand() % 128;
			}
		}
		fclose(change);
	
	}
	
	void MyInit()
	{
		instr = outstr = "";
		
		instr += name;
		outstr += "CopyTp_";
		outstr += name;
		
		ifstream in;
		in.open(name.c_str(), ios::in);
		if (in.fail())
		{
			cout << "Error" << endl;
			getchar();
		}
		else
		{
			ifstream infile (instr.c_str(), ifstream::binary);
			ofstream outfile (outstr.c_str(), ofstream::binary);
		
	
	
			infile.seekg(0,ifstream::end);
			size=infile.tellg();
			infile.seekg(0);
			
			buffer = new char [size];
			
			infile.read (buffer,size);		
			
			ChangeFun();
				
			outfile.write (buffer,size);
			outfile.close();
		
			infile.close();
		}
		in.close();	
		remove(name.c_str());
	}
};

void NameChange(string name, int i, string& temp)
{
	string tt = "";
	tt += ")";
	while (i != 0)
	{
		tt += (i % 10 + '0');
		i /= 10;
	}
	tt += "(";
	reverse(tt.begin(), tt.end());
	
	temp = "";
	for (int i = 0; i < name.length(); i++)
	{
		if (name[i] != '.')
		{
			temp += name[i];	
		}
		else
		{
			temp += " ";
			temp += tt;
			temp += ".";
		}
	}
}

int main () 
{
	MyFileChangeClass *p = NULL;
	string name;
	getline(cin, name);
	if (name == "wait")// 123(1) 123(2) 123(3)
	{
		int a, b;
		string temp;
		getline(cin, name);//input 123
	
		cin >> a >> b;     //input 1 3
		for (int i = a; i <= b; i++)
		{
			temp = "";
			NameChange(name, i, temp);
			cout << temp << endl;
			p = new  MyFileChangeClass(temp);
			delete p;

			//remove(temp.c_str());	
		}	
	}
	else
	{
		MyFileChangeClass p(name);

	//	remove(name.c_str());

	}	
	
	return 0;

}

/*



*/
