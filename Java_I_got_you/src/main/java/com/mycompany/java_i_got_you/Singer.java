/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.java_i_got_you;

import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Anastasiya
 */
public class Singer {
    InetAddress host;
    int port = 3124;
    Thread t_listen;
    Socket cs;
    ObjectInputStream dis;
    
    public Singer() {
        connectToServer();
    }

    public void connectToServer(){
        try {
            host = InetAddress.getLocalHost();
        } catch (UnknownHostException ex) {
            Logger.getLogger(Server.class.getName()).log(Level.SEVERE, null, ex);
        }            
        try { 
            cs = new Socket(host, port); 
            t_listen = new Thread() {
                @Override
                public void run() {
                    InputStream is = null;
                    try {
                        is = cs.getInputStream();
                        dis = new ObjectInputStream(is);
                        while (true) {
                            Object object;
                            try {
                                object = dis.readObject();
                                ArrayList<String> arrayList =  (ArrayList<String>) object;
                                parseResponses(arrayList);
                            } catch (ClassNotFoundException ex) {
                                Logger.getLogger(Singer.class.getName()).log(Level.SEVERE, null, ex);
                            }
                            
                        }
                    } catch (IOException ex) {
                        Logger.getLogger(Singer.class.getName()).log(Level.SEVERE, null, ex);
                    } finally {
                        try {
                            is.close();                            
                        } catch (IOException ex) {
                            Logger.getLogger(Singer.class.getName()).log(Level.SEVERE, null, ex);
                        }
                    }                
                }                
            };
            t_listen.start();
        } catch (IOException ex) {
            Logger.getLogger(Singer.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public void parseResponses(ArrayList<String> list) {
        if (null != list.get(0)) switch (list.get(0)) {
            case "string":
                System.out.println(list.get(1) + ": " + list.get(2));
                break;            
            default:
                break;
        }        
    }
    
    public static void main(String[] args) {
        Singer s = new Singer();
    }
}
