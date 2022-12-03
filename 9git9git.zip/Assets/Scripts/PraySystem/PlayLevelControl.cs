using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLevelControl : MonoBehaviour
{
    [Header("0 = none, 1 = PrayThrow, 2 = PrayRotate, 3 = Dash, 4 = DoubleJump"), SerializeField] private int Level = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (Level)
            {
                case 1:
                    PlayerProperty.PrayThrow = true;
                    break;
                case 2:
                    PlayerProperty.PrayRotate = true;
                    break;
                case 3:
                    PlayerProperty.Dash = true;
                    break;
                case 4:
                    PlayerProperty.DoubleJump = true;
                    break;
                default:
                    break;
            }
           
            Destroy(this.gameObject);
        }
    }
}
