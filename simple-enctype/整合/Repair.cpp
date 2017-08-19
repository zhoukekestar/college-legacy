#include <iostream>
#include <fstream>
#include <string>

using namespace std;

class MyFileRepairClass
{
public:
	MyFileRepairClass(string _name)
	{
		name = _name;
		Repair();
	}
private:
	string name;

	void Repair()
	{
		string	instr = "", outstr = "";
		int insize = -1;
		outstr = "RE_";
		outstr += name;
		instr =  "CopyTp_";
		instr += name;
		
		ifstream in;
		in.open(instr.c_str(), ios::in);
		if (in.fail())
		{
			cout << "Error" << endl;
			getchar();
		}
		else
		{
			ifstream infile(instr.c_str(), ifstream::binary);
			ofstream outfile(outstr.c_str(), ofstream::binary);
			
			char *inbuffer;
			
			infile.seekg(0,ifstream::end);
			insize= infile.tellg();
			infile.seekg(0);
			
			inbuffer = new char[insize];
			
			infile.read(inbuffer, insize);
			
			string str = "CH_";
			str += name;
			str += "temp";
			FILE *p;
			p = fopen(str.c_str(), "r");
			if (p == 0)
			{
				cout << "Error" << endl;
				getchar();
			}
			else
			{
				int num, ch;
				while (~fscanf(p, "%d %d", &num, &ch))
				{
					//cout << num << " " << ch << endl;
					inbuffer[num] = (char)ch;	
				}
				
				outfile.write(inbuffer, insize);
				
			}
			fclose(p);
			remove(str.c_str());
			if (insize != -1)
				delete [] inbuffer;
			infile.close();
		}
		in.close();
		remove(instr.c_str());
		rename(outstr.c_str(), name.c_str());
		
	}

};
void NameChange(string& name, int i, string& temp)
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

int main (int argc, char *argv[]) 
{
	string name;
	name = argv[1];
	MyFileRepairClass *p;
	//getline(cin, name);
	if (name == "wait")// 123(1) 123(2) 123(3)
	{
		int a, b;
		string temp;
		
		name = argv[2];
		a = atoi(argv[3]);
		b = atoi(argv[4]);
		//getline(cin, name);//input 123
		//cin >> a >> b;     //input 1 3
		for (int i = a; i <= b; i++)
		{
			NameChange(name, i, temp);
			cout << temp << endl;
		 	p = new MyFileRepairClass(temp);
			delete p;
		}	
	}
	else
	{
		if (name.length() == 0)
			return 0;
		p = new MyFileRepairClass(name);
		delete p;
	}	 
	
	//MyFileChangeClass *p = new MyFileChangeClass(name);
	return 0;

}

