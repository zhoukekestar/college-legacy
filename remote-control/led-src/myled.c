#include <stdio.h>
#include <stdlib.h> 
#include <unistd.h> 
#include <sys/ioctl.h>
#include <pthread.h>

int is_run = 1;
int  on; 
int  led_no; 
int  fd;

void turn_on(void)
{
	while (is_run == 1)
	{
	on = 1;
	for (led_no = 0; led_no < 4; led_no++)
	ioctl(fd, on, led_no); 
	}
}

void Init()
{
	fd = open("/dev/leds0", 0); 
	if (fd < 0) { 
	 fd = open("/dev/leds", 0); 
	} 
	if (fd < 0) { 
	 perror("open  device  leds"); 
	 exit(1); 
	} 
}
 
int main(int argc, char **argv) 
{ 
Init();
pthread_t id;
int i, ret;
ret = pthread_create(&id, NULL,(void *)turn_on, NULL);

sleep(3);
is_run = 0;
close(fd); 
return  0; 
} 
