#include<unistd.h>
#include<stdio.h>
#include<stdlib.h>
int main(int argc,char *argv[])
{
       int pipedes[2];//pipe输入输出文件描述符
       char s[13]="hello world!";
       int pid;
       if(pipe(pipedes)==-1)//创建管道失败
       {
             perror("pipe error!");
             exit(EXIT_FAILURE);
       }
//	pid = fork();

       if((pid=fork())<0)//创建子进程失败
       {
             perror("fork is error!\n");
             return -1;
       }
       else if(pid==0)//在子进程中
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
       else if(pid  > 0)//在父进程中
       {
            sleep(4);//保证子进程写操作完成
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
