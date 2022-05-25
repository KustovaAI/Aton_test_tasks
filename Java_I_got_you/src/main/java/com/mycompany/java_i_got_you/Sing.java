/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.java_i_got_you;

import java.util.ArrayList;

/**
 *
 * @author Anastasiya
 */
public class Sing {
    int Turn;
    ArrayList<IObserver> all_o = new ArrayList<>();
    int current_string = 0;
    String[][] lyrics = {
        {"Cher", "They say we're young and we don't know \nWe won't find out until we grow"},
        {"Sonny", "Well I don't know if all that's true \n'Cause you got me, and baby I got you"},
        {"Sonny", "Babe"},
        {"Sonny, Cher", "I got you babe \nI got you babe"},
        {"Cher", "They say our love won't pay the rent \nBefore it's earned, our money's all been spent"},
        {"Sonny", "I guess that's so, we don't have a pot \nBut at least I'm sure of all the things we got"},
        {"Sonny", "Babe"},
        {"Sonny, Cher", "I got you babe \nI got you babe"},
        {"Sonny", "I got flowers in the spring \nI got you to wear my ring"},
        {"Cher", "And when I'm sad, you're a clown \nAnd if I get scared, you're always around"},
        {"Cher", "So let them say your hair's too long \n'Cause I don't care, with you I can't go wrong"},
        {"Sonny", "Then put your little hand in mine \nThere ain't no hill or mountain we can't climb"},
        {"Sonny", "Babe"},
        {"Sonny, Cher", "I got you babe \nI got you babe"},
        {"Sonny", "I got you to hold my hand"},
        {"Cher", "I got you to understand"},
        {"Sonny", "I got you to walk with me"},
        {"Cher", "I got you to talk with me"},
        {"Sonny", "I got you to kiss goodnight"},
        {"Cher", "I got you to hold me tight"},
        {"Sonny", "I got you, I won't let go"},
        {"Cher", "I got you to love me so"},
        {"Sonny, Cher", "I got you babe \nI got you babe \nI got you babe \nI got you babe \nI got you babe"}
    };
    
    public void start() {
        while(true) {
            if (current_string <= lyrics.length - 1) {
                switch (lyrics[current_string][0]) {
                    case "Cher" -> all_o.get(0).send_string(lyrics[current_string][0], lyrics[current_string][1]);
                    case "Sonny" -> all_o.get(1).send_string(lyrics[current_string][0], lyrics[current_string][1]);
                    default -> {
                        all_o.get(0).send_string("Cher", lyrics[current_string][1]);
                        all_o.get(1).send_string("Sonny", lyrics[current_string][1]);
                    }
                }             
                current_string++;
                try {
                Thread.sleep(2000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            } else
                break;
        }
    }
    
    public void addO(IObserver o){
        all_o.add(o);        
    }
}
