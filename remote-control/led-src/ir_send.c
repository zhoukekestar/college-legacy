#include     <stdio.h>      /*标准输入输出定义*/
#include     <stdlib.h>     /*标准函数库定义*/
#include     <unistd.h>     /*Unix standrand */
#include     <sys/types.h>  
#include     <sys/stat.h>   
#include     <fcntl.h>      /*file control*/
#include     <termios.h>    /*PPSIX 终端控制定义*/
#include     <errno.h>      /*错误号定义*/
#include 	<sys/ioctl.h>
#include 	<pthread.h>

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
char *msg;
int msg_len;
void start_send()
{
	int fd;
	int nread;
	char buff[512];
	char *dev  = "/dev/ttySAC1"; //串口二
	
	fd = OpenDev(dev);
	set_speed(fd,1200);
	
	
	if (set_Parity(fd,8,1,'N') == FALSE)  
	{
		printf("Set Parity Error\n");
		exit (0);
	}
	printf("send start [%s]\n", msg);
	while (is_running == 1) //循环读取数据
	{   
		nread = write(fd, msg, msg_len);
		sleep(0.5);
	}
	close(fd);
 	exit (0);
}

char Two2One(char *s)
{
	int sum = 0;
	if (s[0] >= '0' && s[0] <= '9')
		sum += s[0] - '0';
	else if (s[0] >= 'a' && s[0] <= 'f')
		sum += s[0] - 'a';
	else
		sum += s[0] - 'A';
		
	sum = sum * 16;
	
	if (s[1] >= '0' && s[1] <= '9')
		sum += s[1] - '0';
	else if (s[1] >= 'a' && s[1] <= 'f')
		sum += s[1] - 'a';
	else
		sum += s[1] - 'A';
	return (char)(sum);
}

void Change2X(char *s)
{
	int i = 0, j = 0;
	msg_len = strlen(s) / 2;
	for (i = 0; i < strlen(s) && i + 1 < strlen(s); i += 2)
	{
		char temp = Two2One(s + i);
		s[j++] = temp;
	}
	s[j] = '\0';
	
}


int main(int argc, char **argv)
{
	msg = argv[1];
	printf("get msg [%s]\n", msg);
	Change2X(msg);
	
	
 	pthread_t id;
	int ret;
	ret = pthread_create(&id, NULL,(void *)start_send, NULL);
	
	char end = getchar();
	is_running=0; 
}