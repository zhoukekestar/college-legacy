#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
/*���������ô򿪹ܵ��ļ���modeΪֻ����ʽ*/
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
       /*���������ܵ�������Ϊ�ɶ�д�����������������������ָ��Ȩ�޴���*/
        if ((mkfifo(FIFO_SERVER, FIFOMODE) < 0) && (errno != EEXIST)) {
                printf("cannot create fifoserver\n");
                exit(1);
        }
        printf("Preparing for reading bytes... ...\n");
	printf("FIFO_SERVER is %s\n", FIFO_SERVER);
        /*�������ܵ��������÷�������־*/
        if ((fd = open(FIFO_SERVER, OPENMODE)) < 0) {
                perror("open");
                exit(1);
        }
        while (1) {
                /*��ʼ��������*/
                bzero(buf, sizeof(buf));
                /*��ȡ�ܵ�����*/
                if ((readnum = read(fd, buf, sizeof(buf))) < 0) {
                        if (errno == EAGAIN) {
                                printf("no data yet\n");
                        }
                }
                /*��������������ӡ���������û�����ݣ������*/
                if (readnum != 0 && strlen(buf) != 0) {
                        buf[readnum] = '\0';
                        printf("read %s from FIFO_SERVER\n", buf);
                }
                sleep(1);
        }
        return 0;
}
