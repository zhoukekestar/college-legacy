package ChatMain;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

public class MySocket {

	static void SendMes(String a_Msg) {
		try {
			Socket socket = new Socket(Config.Addr,
					Integer.parseInt(Config.Port));

			PrintWriter printWriter = new PrintWriter(socket.getOutputStream());
			printWriter.println(a_Msg);
			printWriter.flush();
			printWriter.close();

			socket.close();
		} catch (Exception e) {

		}
	}

	static String GetMsg() {
		try {
			Config.sSocket = new ServerSocket(
					Integer.parseInt(Config.Port));
			
			Socket socket = Config.sSocket.accept();

			BufferedReader in = new BufferedReader(new InputStreamReader(
				socket.getInputStream()));	
								
			String temp = "";
			
			int i;
	        while ((i = in.read())>-1) {
	             temp+=(char)i;
	        }
            //temp = in.readLine();
            
	        in.close();			
			socket.close();
			Config.sSocket.close();
			
			return temp;
		} catch (NumberFormatException | IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return "";
	}

}
