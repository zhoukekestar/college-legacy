/*http://wenku.baidu.com/view/758d6422af45b307e87197ed.html*/


#ifndef _MagicCube_H_ 
#define _MagicCube_H_  

#include <stdio.h> 
#include <stdlib.h> 
#include <gl/glaux.h> 
#include <math.h> 
#include <gl/glut.h> 

#define PI 3.141596   

typedef struct 
{  
	GLfloat Xpos[8];   //x coord of 8 pointers  
	GLfloat Ypos[8];  
	GLfloat Zpos[8];   
	GLfloat Center[3]; //Center pos (x,y,z)  
}SmallCube;  

SmallCube SmCube[27];   
GLuint Texture[27];  

void CubeInit();   //initial of 8 SmCube  
void MathInit();  

void EntireRot(); 
void EntireRotAng();  

void PartRot();   //rand the spin axis 
void ProPartRotX(int iCube);    
void ProPartRotY(int iCube); 
void ProPartRotZ(int iCube); 
void PointRot(GLfloat *X1,GLfloat *X2,GLfloat *X3);  

void StartEffect(); 
void DrawSmCube(); 
void DrawMgCube();  
GLfloat Point[8][3]=            //point coord of a SmCube 
{  
	{ -1.0f, -1.0f, 1.0f},   //0   --  front 
	{  1.0f, -1.0f, 1.0f},   //1   
	{  1.0f,  1.0f, 1.0f},   //2 
	{ -1.0f,  1.0f, 1.0f},   //3  

	{ -1.0f, -1.0f, -1.0f}, //4  --back  
	{ -1.0f,  1.0f, -1.0f}, //5  
	{  1.0f,  1.0f, -1.0f}, //6 
	{  1.0f, -1.0f, -1.0f}, //7   
};   
GLfloat CenterInit[3]={0,0,0};  //init center point   
//point sequence  of SmCube face                              
GLint Face[6][4]={{0,1,2,3},{4,5,6,7},{3,2,6,5},    //front ,back ,up      
{0,4,7,1},{1,7,6,2},{0,3,5,4}};  // bottom, right ,left  
GLint TexCd[4][2]={{0,0},{1,0},{1,1},{0,1}};        //texture coord array for one square  

int xAng=0;   //entire rotate angle 
int yAng=0; 
int zAng=0;  

int PartRotAng=0;   //remember the part rotate ang 
int Axis=0;      //mark the spin axis  
GLfloat sina=0; 
GLfloat cosa=0;  
int Start=1;   
#endif    