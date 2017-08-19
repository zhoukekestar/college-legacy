#include<unistd.h>
#include<stdio.h>
#include<stdlib.h>
int main(int argc,char *argv[])
{
       int pipedes[2];//pipe��������ļ�������
       char s[13]="hello world!";
       int pid;
       if(pipe(pipedes)==-1)//�����ܵ�ʧ��
       {
             perror("pipe error!");
             exit(EXIT_FAILURE);
       }
//	pid = fork();

       if((pid=fork())<0)//�����ӽ���ʧ��
       {
             perror("fork is error!\n");
             return -1;
       }
       else if(pid==0)//���ӽ�����
       {
             printf("now,write data to pipe\n");
             if(write(pipedes[1],s,13)==-1)
             {
                     perror("write error!");
                     exit(EXIT_FAILURE);
             }
             else
             {
                    printf("the written data is :%s\n",s);
                    exit(EXIT_SUCCESS);
             }
       }
       else if(pid  > 0)//�ڸ�������
       {
            sleep(4);//��֤�ӽ���д�������
            printf("now read data from pipe!\n");
            if(read(pipedes[0],s,13)==-1)
            {
                    perror("read error!");
                    exit(EXIT_FAILURE);   
            }
            printf("the data is %s\n",s);
       }
         
       return 0;

}     
