using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevBoulder : MonoBehaviour
{
   public bool levitate;


    [SerializeField] float speed;
    [SerializeField] int pathStartPoint;
    [SerializeField] Transform[] points;


    int p;
    bool reverse;


    void Start()
    {
        transform.position = points[pathStartPoint].position;
        p = pathStartPoint;
    }


    void Update()
    {
        if(Vector3.Distance(transform.position, points[p].position) < 0.01f)
        {
            levitate = false;
            if (p == points.Length - 1)
            {
                reverse = true;
                p--;
                return;
            }


            else if(p == 0)
            {
                reverse = true;
                p++;
                return;
            }


            if (reverse)
            {
                p--;
            }


            else
            {
                p++;
            }
        }
            if (levitate)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[p].position, speed * Time.deltaTime);


            }
         }
    }
