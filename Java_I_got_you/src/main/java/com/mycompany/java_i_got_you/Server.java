/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.java_i_got_you;

import java.io.IOException;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Anastasiya
 */
public class Server {
    int port = 3124;
    InetAddress host;
    Sing sing;
    
    public Server() {
        
        try {
            host = InetAddress.getLocalHost();
        } catch (UnknownHostException ex) {
            Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
        }
            
        try {    
            ServerSocket ss = new ServerSocket(port, 0, host);
            System.out.println("Server started");
            sing = new Sing();
            int count = 0;
            try {
            while (true) {
                Socket cs = ss.accept();  
                count++;
                System.out.println("Client " + count + " connect");
                ClientThread cc = new ClientThread(cs, sing);
                if (count == 2)
                    sing.start();
            }
            } catch (IOException ex) {
            Logger.getLogger(ClientThread.class.getName()).log(Level.SEVERE, null, ex);
            ss.close();
        }
        } catch (IOException ex) {
            Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public static void main(String[] args) {
        Server server = new Server();
    } 
}
