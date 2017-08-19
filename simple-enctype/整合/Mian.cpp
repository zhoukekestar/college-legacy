#include<stdio.h>   
#include <string.h>
#include <windows.h>
#include <stdlib.h>
#include <string>
#include <iostream>

using namespace std;

char s[100] = "";

void FUN(int num)
{
	strcpy(s, "");
	char ch[2];
	if (num == 1)
		strcat(s, "_Change.exe ");
	else
		strcat(s, "_Repair.exe ");

	string str;
	int a, b;
	puts("..............");
	puts(". 1 for one  .");
	puts(". 2 for more .");
	puts("..............");
	cin >> ch;
	getchar();
	if (ch[0] == '1')
	{
		puts("Input file name:");
		getline(cin, str);	
	
		str.insert(0, "\"");
		str += "\"";
		
		strcat(s, str.c_str());		  	
	}
	else
	{
		puts("Input file name:");
		getline(cin, str);
		
		str.insert(0, " \"");
		str += "\"";
		
		strcat(s, " \"wait\"");
		strcat(s, str.c_str());
		
		puts("From a to b.");
		cin >> a >> b;		
		
		char buffer[100];
		str = "";
		itoa(a, buffer, 10);
		str += buffer;
		str.insert(0, " \"");
		str += "\"";
		strcat(s, str.c_str());
		
		str = "";
		itoa(b, buffer, 10);
		str += buffer;
		str.insert(0, " \"");
		str += "\"";
		strcat(s, str.c_str());

	}
}

int  main(void)   
{
	while(1)   
	{
		puts("................");
		puts(". c for change .");
		puts(". r for repair .");
		puts("................");
		
		char ch[2];
		scanf("%s", ch);
		if (ch[0] == 'c') 		FUN(1);
		else if (ch[0] == 'r') 	FUN(0);
		else					puts("Error");
		
		system(s);
		puts("");
	}

	return 0;     
} 