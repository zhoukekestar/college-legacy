#include     <stdio.h>      /*标准输入输出定义*/
#include     <stdlib.h>     /*标准函数库定义*/
#include     <unistd.h>     /*Unix standrand */
#include     <sys/types.h>  
#include     <sys/stat.h>   
#include     <fcntl.h>      /*file control*/
#include     <termios.h>    /*PPSIX 终端控制定义*/
#include     <errno.h>      /*错误号定义*/
#include <sys/ioctl.h>
#include <pthread.h>

#define FALSE  -1
#define TRUE   0
/*********************************************************************/

int speed_arr[] = { B38400, B19200, B9600, B4800, B2400, B1200, B300,
          B38400, B19200, B9600, B4800, B2400, B1200, B300, };
int name_arr[] = {38400,  19200,  9600,  4800,  2400,  1200,  300, 38400,  
          19200,  9600, 4800, 2400, 1200,  300, };
void set_speed(int fd, int speed){
  int   i; 
  int   status; 
  struct termios   Opt;
  tcgetattr(fd, &Opt); 
  for ( i= 0;  i < sizeof(speed_arr) / sizeof(int);  i++) 
  { 
    if  (speed == name_arr[i]) 
	{     
      tcflush(fd, TCIOFLUSH);     
      cfsetispeed(&Opt, speed_arr[i]);  
      cfsetospeed(&Opt, speed_arr[i]);   
      status = tcsetattr(fd, TCSANOW, &Opt);  
      
      if  (status != 0) 
	  {        
        perror("tcsetattr fd1");  
        return;     
      }    
      tcflush(fd,TCIOFLUSH);   
    }  
  }
}

int set_Parity(int fd,int databits,int stopbits,int parity)
{ 
	struct termios options; 
	if  ( tcgetattr( fd,&options)  !=  0) { 
		perror("SetupSerial 1");     
		return(FALSE);  
	}
	options.c_cflag &= ~CSIZE; 
	
	switch (databits) /*set data bit*/
	{   
	case 7:		
		options.c_cflag |= CS7; 
		break;
	case 8:     
		options.c_cflag |= CS8;
		break;   
	default:    
		fprintf(stderr,"Unsupported data size\n"); return (FALSE);  
	}
	
	switch (parity) 
	{   
	case 'n':
	case 'N':    
		options.c_cflag &= ~PARENB;   /* Clear parity enable */
		options.c_iflag &= ~INPCK;     /* Enable parity checking */ 
		break;  
	case 'o':   
	case 'O':     
		options.c_cflag |= (PARODD | PARENB); /* ??????*/  
		options.c_iflag |= INPCK;             /* Disnable parity checking */ 
		break;  
	case 'e':  
	case 'E':   
		options.c_cflag |= PARENB;     /* Enable parity */    
		options.c_cflag &= ~PARODD;   /* ??????*/     
		options.c_iflag |= INPCK;       /* Disnable parity checking */
		break;
	case 'S': 
	case 's':  /*as no parity*/   
	    options.c_cflag &= ~PARENB;
		options.c_cflag &= ~CSTOPB;break;  
	default:   
		fprintf(stderr,"Unsupported parity\n");    
		return (FALSE);  
	}  
	/* ?????*/  
	switch (stopbits)
	{   
		case 1:    
			options.c_cflag &= ~CSTOPB;  
			break;  
		case 2:    
			options.c_cflag |= CSTOPB;  
		   break;
		default:    
			 fprintf(stderr,"Unsupported stop bits\n");  
			 return (FALSE); 
	} 
	/* Set input parity option */ 
	if (parity != 'n')   
		options.c_iflag |= INPCK; 
	tcflush(fd,TCIFLUSH);
	options.c_cc[VTIME] = 150; /* ????15 seconds*/   
	options.c_cc[VMIN] = 0; /* Update the options and do it NOW */
	if (tcsetattr(fd,TCSANOW,&options) != 0)   
	{ 
		perror("SetupSerial 3");   
		return (FALSE);  
	} 
	return (TRUE);  
}

int OpenDev(char *Dev)
{
	//open with R/W method
	int	fd = open( Dev, O_RDWR );         //| O_NOCTTY | O_NDELAY	
	if (-1 == fd)	
	{
		//output the error infomation
		perror("Can't Open Serial Port");
		return -1;		
	}	
	else	
		return fd;
}

int is_running = 1;
void start_listen()
{
	int fd;
	int nread;
	char buff[512];
	char *dev  = "/dev/ttySAC2"; //串口二
	
	fd = OpenDev(dev);
	set_speed(fd,1200);
	
	
	if (set_Parity(fd,8,1,'N') == FALSE)  
	{
		printf("Set Parity Error\n");
		exit (0);
	}
	printf("read start...\n");
	while (is_running == 1) //循环读取数据
	{   
		while((nread = read(fd, buff, 512))>0 && is_running == 1)
		{ 
			printf("\nLen %d\n",nread); 
			buff[nread+1] = '\0';  
			
			int i;
			for (i = 0; i < nread; i++)
			printf("%d:%X ",i, buff[i]);   
			puts("");
		}
	}
	close(fd);
 	exit (0);
}
int main(int argc, char **argv)
{
 	pthread_t id;
	int ret;
	ret = pthread_create(&id, NULL,(void *)start_listen, NULL);
	
	char end = getchar();
	is_running=0; 
}