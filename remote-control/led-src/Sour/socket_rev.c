#include<stdlib.h>
#include<stdio.h>
#include<errno.h>
#include<string.h>
#include<netdb.h>
#include<sys/types.h>
#include<netinet/in.h>
#include<sys/socket.h>

int main(int argc,char *argv[])
{
	
  char SERVER_IP[1000] = "220.170.79.208";
  char PORT[10] = "16258";
  
  if (argc == 3)
  {
  	strcpy(SERVER_IP, argv[1]);
  	strcpy(PORT, argv[2]);
  }
  int sockfd;
  char buffer[1024];
  struct sockaddr_in client_addr;
  struct hostent *host;
  int nbytes;

  sockfd = socket(AF_INET, SOCK_STREAM, 0); 
  if(sockfd	< 0)
  {
    fprintf(stderr,"Socket error:%s\a\n",strerror(errno));
    exit(1);
  }
  printf("Socket id is %d\n",sockfd);

  bzero(&client_addr,sizeof(struct sockaddr_in));

  client_addr.sin_family=AF_INET;
  client_addr.sin_port=htons(atoi(PORT));
  client_addr.sin_addr.s_addr=inet_addr(SERVER_IP);

  printf("Set OK!\n");
  
  int err = connect(sockfd,(struct sockaddr *)(&client_addr),sizeof(struct sockaddr));
  printf("Connect Result OK!\n");
  if(err == -1)
  {
    fprintf(stderr,"Connect Error:%s\n\a",strerror(errno));
    exit(1);
  }
  printf("Connect\n");
  printf("REV [SERVER_EXIT] to EXIT");
  while (1)
  {
	  nbytes=recv(sockfd,buffer,1024,0);
	  if(-1==nbytes){
	    fprintf(stderr,"Read Error:%s\n\a",strerror(errno));
	    exit(1);
	  }
	  buffer[nbytes]='\0';
	  if (strcmp(buffer, "SERVER_EXIT") == 0)
	  	break;
	  printf("I have received:%s\n",buffer);
	  
	  char cmd_temp[1000];
	  sprintf(cmd_temp, "echo '%s' > socket_rev.temp", buffer);
	  system(cmd_temp);
	  
	  buffer[nbytes++] = '&';
	  buffer[nbytes++] = 'O';
	  buffer[nbytes++] = 'K';
	  buffer[nbytes] = '\0';
	  
	  send(sockfd, buffer, nbytes, 0);	
  }
  close(sockfd);
  exit(0);
}