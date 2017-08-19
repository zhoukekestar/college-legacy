#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
/*在这里设置打开管道文件的mode为只读形式*/
#define FIFOMODE (O_CREAT | O_RDWR | O_NONBLOCK)
#define OPENMODE (O_RDONLY | O_NONBLOCK)
int main(int argc, int **argv)
{
        char buf[100];
	char* FIFO_SERVER = "MYCGI_SERVER_2";
	if (argc == 2)
		FIFO_SERVER = argv[1];
        int fd;
        int readnum;
       /*创建有名管道，设置为可读写，无阻塞，如果不存在则按照指定权限创建*/
        if ((mkfifo(FIFO_SERVER, FIFOMODE) < 0) && (errno != EEXIST)) {
                printf("cannot create fifoserver\n");
                exit(1);
        }
        printf("Preparing for reading bytes... ...\n");
	printf("FIFO_SERVER is %s\n", FIFO_SERVER);
        /*打开有名管道，并设置非阻塞标志*/
        if ((fd = open(FIFO_SERVER, OPENMODE)) < 0) {
                perror("open");
                exit(1);
        }
        while (1) {
                /*初始化缓冲区*/
                bzero(buf, sizeof(buf));
                /*读取管道数据*/
                if ((readnum = read(fd, buf, sizeof(buf))) < 0) {
                        if (errno == EAGAIN) {
                                printf("no data yet\n");
                        }
                }
                /*如果读到数据则打印出来，如果没有数据，则忽略*/
                if (readnum != 0 && strlen(buf) != 0) {
                        buf[readnum] = '\0';
                        printf("read %s from FIFO_SERVER\n", buf);
                }
                sleep(1);
        }
        return 0;
}
