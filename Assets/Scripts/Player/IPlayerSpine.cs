using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  public interface IPlayerSpine { 

    void up_idle_playing(); 
    void move_up_playing(); 
    void side_idle_playing(); 
    void move_side_playing();
    void down_idle_playing(); 
    void move_down_playing();
    void attack_start();
  
    void direction(int direction);
    string get_name();

  }

