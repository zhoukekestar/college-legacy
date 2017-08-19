#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <string.h>
#include <unistd.h>
#include <errno.h>

/*特别注意写管道时，设置打开管道文件的格式必须为可写*/
#define FIFO_SERVER "MYCGI_SERVER_2"
#define OPENMODE (O_WRONLY | O_NONBLOCK)

int main(int argc, char **argv)
{
        int fd;
        int nwrite;

        /*打开管道文件，可写非阻塞*/
        if ((fd = open(FIFO_SERVER, OPENMODE)) < 0) {
                perror("open");
                exit(1);
        }

        /*如果没有在命令行中写入参数，那么要重新运行程序*/
        if (argc == 1) {
                printf("Please send something\n");
                exit(1);
        }

        /*向管道文件中写入数据，在这里要用strlen，如果用sizeof，则只是4个字节的指针长度*/
        if ((nwrite = write(fd, argv[1], strlen(argv[1]))) < 0) {
                if (errno == EAGAIN) {
                        printf("The FIFO has not been read yet.Please try later\n");
                }
        }
        else {
                printf("write %s to FIFO\n", argv[1]);
        }

        return 0;
}
