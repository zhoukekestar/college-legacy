#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <string.h>
#include <unistd.h>
#include <errno.h>

/*�ر�ע��д�ܵ�ʱ�����ô򿪹ܵ��ļ��ĸ�ʽ����Ϊ��д*/
#define FIFO_SERVER "MYCGI_SERVER_2"
#define OPENMODE (O_WRONLY | O_NONBLOCK)

int main(int argc, char **argv)
{
        int fd;
        int nwrite;

        /*�򿪹ܵ��ļ�����д������*/
        if ((fd = open(FIFO_SERVER, OPENMODE)) < 0) {
                perror("open");
                exit(1);
        }

        /*���û������������д���������ôҪ�������г���*/
        if (argc == 1) {
                printf("Please send something\n");
                exit(1);
        }

        /*��ܵ��ļ���д�����ݣ�������Ҫ��strlen�������sizeof����ֻ��4���ֽڵ�ָ�볤��*/
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
