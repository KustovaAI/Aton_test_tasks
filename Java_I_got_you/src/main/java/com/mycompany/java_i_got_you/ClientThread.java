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
import java.net.Socket;
import java.util.ArrayList;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Anastasiya
 */
public class ClientThread implements IObserver{
    Socket cs;
    OutputStream os;          
    ObjectOutputStream dos;
    Sing sing;
    
    public ClientThread(Socket cs, Sing s) {
        this.cs = cs;
        this.sing = s;
        try {            
            os = cs.getOutputStream();
            dos = new ObjectOutputStream(os);
        } catch (IOException ex) {
            Logger.getLogger(ClientThread.class.getName()).log(Level.SEVERE, null, ex);
        }
        s.addO(this);        
    }

    
    public void send(Object o) {
        try { 
            dos.writeObject(o);
        } catch (IOException ex) {
            Logger.getLogger(ClientThread.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    @Override
    public void send_string(String name, String text) {
        ArrayList<String> arrayList =  new ArrayList<>();
        arrayList.add("string");
        arrayList.add(name);
        arrayList.add(text);
        send(arrayList);
    }    
    
}
