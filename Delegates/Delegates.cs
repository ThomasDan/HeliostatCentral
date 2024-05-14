using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTrackerCentral.Delegates
{
    /* ################## DELEGATORS ####################
                         .--.  .--.
                        /    \/    \
                       | .-.  .-.   \
                       |/_  |/_  |   \
                       || `\|| `\|    `----.
                       |\0_/ \0_/    --,    \_
     .--"""""-.       /              (` \     `-.
    /          \-----'-.              \          \
    \  () ()                         /`\          \
    |                         .___.-'   |          \
    \                        /` \|      /           ;
     `-.___             ___.' .-.`.---.|             \
        \| ``-..___,.-'`\| / /   /     |              `\
         `      \|      ,`/ /   /   ,  /
                 `      |\ /   /    |\/
                  ,   .'`-;   '     \/
             ,    |\-'  .'   ,   .-'`
           .-|\--;`` .-'     |\.'
          ( `"'-.|\ (___,.--'`'   
           `-.    `"`          _.--'
              `.          _.-'`-.
                `''---''``       `.
    */
    public delegate Task SaveReceivedMessageDelegate(string message);
}
