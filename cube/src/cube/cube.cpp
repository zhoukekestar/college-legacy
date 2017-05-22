

#include "stdafx.h"
#include "MagicCube.h"
#include <stdio.h> 
#include <stdlib.h> 
#include <gl/glaux.h> 
#include <math.h> 
#include <gl/glut.h> 

void WindowsInit() 
{  
	glClearColor(0,0,0,1);  
	glClear(GL_DEPTH_BUFFER_BIT|GL_COLOR_BUFFER_BIT);  
	glCullFace(GL_FRONT);  
	glEnable(GL_DEPTH_TEST);  
	glEnable(GL_CULL_FACE); 
	//---------------------------------------------------2

	glEnable(GL_TEXTURE_2D);  //smooth the cube  
	glEnable(GL_POLYGON_SMOOTH);  
	glHint(GL_POLYGON_SMOOTH,GL_NICEST);  
	glEnable(GL_BLEND);    
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);   
	MathInit();  
	CubeInit(); 
} 
void MathInit() 
{  
	GLfloat a=PI/180 ;   //1 degree for one spin   
	sina=sin(a);  
	cosa=cos(a); 
}  
void CubeInit()     //initial of pointers and center position 
{  
	int i,j,k;  
	int iCube=0;  
	int iPoint=0;  
	GLfloat x,y,z;  
	for(i=0,x=2;i<3;i++,x-=2)     
		for(j=0,y=2;j<3;j++,y-=2)     
			for(k=0,z=2;k<3;k++,z-=2)  
			{   
				SmCube[iCube].Center[0]=CenterInit[0]+x;  //init center (x,y,z) of SmCube[iCube]   
				SmCube[iCube].Center[1]=CenterInit[1]+y;   
				SmCube[iCube].Center[2]=CenterInit[2]+z;   
				for(iPoint=0;iPoint<8;iPoint++)         //init 8 points (x,y,z) of SmCube[iCube]   
				{    
					SmCube[iCube].Xpos[iPoint]=Point[iPoint][0]+x;    
					SmCube[iCube].Ypos[iPoint]=Point[iPoint][1]+y;    
					SmCube[iCube].Zpos[iPoint]=Point[iPoint][2]+z;    
				}   
				iCube++;   
			}     
}  
/*
AUX_RGBImageRec *LoadBMP(char *Filename) 
{  
	FILE *File=NULL; 

	//--------------------------------------------------------3

	if(!Filename)  
	{      
		return NULL;  
	} 
	fopen_s(&File, Filename, "r");

	if(File)  
	{   
		fclose(File);   
		return auxDIBImageLoad((LPCWSTR)Filename);  //BMP structure  
	}  
	return NULL;   
	//lpcwstr
} 
int LoadGLTextures() 
{  
	int Status=FALSE;  
	int i;  
	char *Picture[]={"DataBMP/BMP1.bmp","DataBMP/BMP2.bmp","DataBMP/BMP3.bmp","DataBMP/BMP4.bmp","DataBMP/BMP5.bmp",
	"DataBMP/BMP1.bmp","DataBMP/BMP2.bmp","DataBMP/BMP3.bmp","DataBMP/BMP4.bmp","DataBMP/BMP5.bmp"
	"DataBMP/BMP1.bmp","DataBMP/BMP2.bmp","DataBMP/BMP3.bmp","DataBMP/BMP4.bmp","DataBMP/BMP5.bmp"
	"DataBMP/BMP1.bmp","DataBMP/BMP2.bmp","DataBMP/BMP3.bmp","DataBMP/BMP4.bmp","DataBMP/BMP5.bmp"
	"DataBMP/BMP1.bmp","DataBMP/BMP2.bmp","DataBMP/BMP3.bmp","DataBMP/BMP4.bmp","DataBMP/BMP5.bmp"
	"DataBMP/BMP1.bmp","DataBMP/BMP2.bmp"};//27  
	AUX_RGBImageRec *TextureImage[27];  
	memset(TextureImage,'0',sizeof(void*)*27);  
	for(i=0;i<27;i++)  
	{   
		if(TextureImage[i]=LoadBMP(Picture[i])) 
			//------------------------------------------------------4

		{    
			Status=TRUE;    
			glGenTextures(1,&Texture[i]);    
			glBindTexture(GL_TEXTURE_2D,Texture[i]);    
			glTexImage2D(GL_TEXTURE_2D,0,3,TextureImage[i]->sizeX,TextureImage[i]->sizeY,     
				0,GL_RGB,GL_UNSIGNED_BYTE,TextureImage[i]->data);    
			glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);    
			glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);        
			glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);   
		}   
		if(TextureImage[i])   
		{    
			if(TextureImage[i]->data)    
			{     
				free(TextureImage[i]->data);    
			}    
			free(TextureImage[i]);   
		}  
	}  
	return Status;   
}  */
void DrawSmCube(int i) 
{  

	int x = 1, y = 1, z =1;
	int iFace,iPoint;    //6 faces,4 points  
	//glBindTexture(GL_TEXTURE_2D,Texture[i]);    
	for(iFace=0;iFace<6;iFace++)  
	{  
		
		//glColor3f(x, y, z);
		glBindTexture(GL_TEXTURE_2D,Texture[iFace]);   
		glBegin(GL_QUADS);    
		
		for(iPoint=0;iPoint<4;iPoint++)   
		{    
			glTexCoord2f(TexCd[iPoint][0],TexCd[iPoint][1]);  //texture coord of point of face[iFace]    
			//glColor3f(x,0, 0);
			glVertex3f(SmCube[i].Xpos[Face[iFace][iPoint]],  //x of of Point[iPoint]  of Face[iFace]             
			SmCube[i].Ypos[Face[iFace][iPoint]],  //y of of Point[iPoint]  of Face[iFace]             
			SmCube[i].Zpos[Face[iFace][iPoint]]);  //z of of PointiPoint]  of Face[iFace]  
		} 
		//------------------------------------------------------5
		//x -= 0.1;
		//y -= 0.2;
		//z -= 0.3;
		glEnd();  
	}     
} 
void DrawMgCube() 
{  
	int iCube=0;  
	for(iCube=0;iCube<27;iCube++)  
	{   
		DrawSmCube(iCube);  
	}
}  
void StartEffect() 
{  
	int iCube=0;  
	int  iStart=0;  
	GLfloat xP=9;  
	GLfloat xN=-9;  
	GLfloat yP=9;  
	GLfloat yN=-9;  
	glClearColor(0,0,0,1);    
	for(iStart=0;iStart<90;iStart++)  
	{      
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);  
		//*************   
		glPushMatrix();   
		EntireRot();   
		EntireRot();   
		glTranslatef(xP,yP,0);   
		EntireRot();      
		DrawMgCube();   
		glPopMatrix();   
		//*****************   
		glPushMatrix();   
		EntireRot();   
		EntireRot();   
		glTranslatef(xN,yN,0);   
		EntireRot();
		DrawMgCube();   
		glPopMatrix();   
		//*******************   
		glPushMatrix();   
		EntireRot();   
		EntireRot();   
		glTranslatef(xP,yN,0);   
		EntireRot();      
		DrawMgCube();   
		glPopMatrix();   
		//*******************   
		glPushMatrix();   
		EntireRot();   
		EntireRot();   
		glTranslatef(xN,yP,0);   
		EntireRot();  
		DrawMgCube();   
		glPopMatrix();   
		//*********************  
		EntireRotAng();   
		glutSwapBuffers();   
		xP=xP-0.1;   
		yP=yP-0.1;      
		xN=xN+0.1;   
		yN=yN+0.1;     
	}     
} 
void Display() 
{  
	glClearColor(0,0,0,1);  

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);  
	if(Start==1)  
	{  
		StartEffect();   
		Start=0;  
	}  
	else  
	{   
		glPushMatrix();   
		EntireRot(); 
		//----------------------------------------------------7

		DrawMgCube();      
		glPopMatrix();   
		PartRot();   
		glutSwapBuffers();   
		EntireRotAng();     
	} 
}  

void EntireRot() 
{  
	glRotatef(xAng,1,0,0);  
	glRotatef(yAng,0,1,0);  
	glRotatef(zAng,0,0,1); 
}  

void EntireRotAng()
{  
	xAng=xAng+1;  
	yAng=yAng+1;  
	zAng=zAng+1;  
	if(xAng>360)   xAng=0;  
	if(yAng>360)   yAng=0;     
	if(zAng>360)   zAng=0; 
} 

void PartRot() 
{  
	int iCube=0;  
	if(PartRotAng==0)  
	{   
		Axis=rand()%9;  
		printf("Axis=%d\n",Axis);  
	}  
	PartRotAng+=1;  
	if(PartRotAng==90)  //1 degree for one time   
			PartRotAng=0;   
	for(iCube=0;iCube<27;iCube++)    
	{
		//----------------------------------------------8
		if(Axis==0&&SmCube[iCube].Center[0]>1)
			//rotate around x,x>1    
				ProPartRotX(iCube);    
		if(Axis==1&&SmCube[iCube].Center[0]<0.5&&SmCube[iCube].Center[0]>-0.5)
			//rotate around x,-0.5<x<0.5    
				ProPartRotX(iCube);   
		if(Axis==2&&SmCube[iCube].Center[0]<-1)
			//rotate around x,x<-1    
				ProPartRotX(iCube);   
		if(Axis==3&&SmCube[iCube].Center[1]>1)//rotate around y,y>1    
			ProPartRotY(iCube);   
		if(Axis==4&&SmCube[iCube].Center[1]<0.5&&SmCube[iCube].Center[1]>-0.5)
			//rotate around y ,-0.5<Y<0.5    
				ProPartRotY(iCube);   
		if(Axis==5&&SmCube[iCube].Center[1]>-1)//rotate around y,y<-1    
			ProPartRotY(iCube);   
		if(Axis==6&&SmCube[iCube].Center[2]>1)//rotate around z,z>1    
			ProPartRotZ(iCube);   
		if(Axis==7&&SmCube[iCube].Center[2]<0.5&&SmCube[iCube].Center[2]>-0.5)//rotate around z ,-0.5<z<0.5    

			ProPartRotZ(iCube);   
		if(Axis==8&&SmCube[iCube].Center[2]<-1)//rotate around z,z<-1    
			ProPartRotZ(iCube);  
	}  
}  


void ProPartRotX(int iCube) 
{  
	int i;  //Center (x,y,z) rotate  
	PointRot(&SmCube[iCube].Center[0],&SmCube[iCube].Center[1],&SmCube[iCube].Center[2]);  
	for(i=0;i<8;i++)                //process 8 points rotate  
	{   
		PointRot(&SmCube[iCube].Xpos[i],&SmCube[iCube].Ypos[i],&SmCube[iCube].Zpos[i]); 
	}    
}   

void ProPartRotY(int iCube) 
{  
	int i;  //Center (x,y,z) rotate  
	PointRot(&SmCube[iCube].Center[1],&SmCube[iCube].Center[2],&SmCube[iCube].Center[0]); 

	for(i=0;i<8;i++)                 //process 8 points rotate  
	{   
		PointRot(&SmCube[iCube].Ypos[i],&SmCube[iCube].Zpos[i],&SmCube[iCube].Xpos[i]);  
	}  
} 

void ProPartRotZ(int iCube) 
{  
	int i;  //Center (x,y,z) rotate  
	PointRot(&SmCube[iCube].Center[2],&SmCube[iCube].Center[0],&SmCube[iCube].Center[1]);  
	for(i=0;i<8;i++)                 //process 8 points rotate  
	{   
		PointRot(&SmCube[iCube].Zpos[i],&SmCube[iCube].Xpos[i],&SmCube[iCube].Ypos[i]);  
	}    
}  

void PointRot(GLfloat *X1,GLfloat *X2,GLfloat *X3) 
{  
	GLfloat TempX2;  
	TempX2=(*X2);  
	*X1=(*X1);  
	*X2=(*X2)*cosa-(*X3)*sina;  
	*X3=TempX2*sina+(*X3)*cosa; 
}   

void Reshape(int w,int h) 
{  
	GLfloat nRange=18; // GLfloat Aspect=0;    
	if(h==0)  
		h=1;  
	glViewport(0,0,w,h);  
	//Aspect=(float)w/(float)h;  
	glMatrixMode(GL_PROJECTION);  
	glLoadIdentity();     //gluPerspective(60,Aspect,10,500);     
	// gluLookAt(20,0,0,0,0,0,0,1,0);

	//--------------------------------------------10

	if(w<=h)  
	{   
		glOrtho(-nRange,nRange,-nRange*h/w,nRange*h/w,nRange*2,-nRange*2); 
	}  
	else  
	{   
		glOrtho(-nRange*w/h,nRange*w/h,-nRange,nRange,nRange*2,-nRange*2);  
	}      
	glMatrixMode(GL_MODELVIEW);  
	glLoadIdentity(); 
}  

void TimerFunc(int value) 
{ 
	glutPostRedisplay();  
	glutTimerFunc(50,TimerFunc,1); 
}  

int _tmain(int argc,char **argv)
{  

	glutInit(&argc,argv); 
	
	glutInitDisplayMode(GLUT_DOUBLE|GLUT_RGB|GLUT_DEPTH);  
	glutInitWindowSize(800,800);  
		glutInitWindowPosition(100,100);  
		glutCreateWindow("MagicCube"); 
	//while(true)
	{
		

		
		   
		WindowsInit();  
		//LoadGLTextures();  
	
		glutDisplayFunc(Display);  
		glutReshapeFunc(Reshape);  
		glutTimerFunc(50,TimerFunc,1);  
	}
	glutMainLoop();  
	return 0;   
} 