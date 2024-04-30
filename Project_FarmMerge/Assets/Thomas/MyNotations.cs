//MY NOTATIONS

//THE GOAL IS TO COMPLETE IN A CYCLE OF 25 DAYS
//create the system for the player to contrl  when and where the ball is dropped
//create the system for choosing the next ball
//create the system for any balls that can to merge when in contact.
//the player gains score for each merge and each placed;
//when balls dont interact they have a tendecy to roll and fall to the sides.
//you can destroy all small fellas
//you can destroy a target after watching an ad.
//you can envolve a target.
//then we will create a farm which grows by achieving certain things in the game
//



//GOAL
///make it grow
///make it bounce
///make line indicating where it will fall
///make ui below showing all combinations
///show queue for next 3 


//GOAL
///make sure the 2d stuff is working
///put the graphics
///create the order table
///make sure the game is running as it was.
///merge effect. the one colliding kinda moves behind the one collided and both disappear toi show the next


//GOAL
///create destroy target power.
///create upgrade taret
///create destroy all below chicken power
///create confirmation menu



//GOAL 
///you gain additional point for every merge done per turn. a turn is what happened between you dropping the ball (its only showing 1X) (and it should not be in)
///you get words of exciment for every merge that become better based on turn perfomance.
///i will create two especial fellas: the fox which goes directly to the ground and destroy all animals below cat. awrwds points
///create a button for cancelling a power
///improve the target ui. make it appear in the built version.
///create a line showing where teh fella will fall.
///need to find a way to make all the colliders adapt to the screen size

//GOAL
//show the queue list.
//make it easier to control it by allowing the player to drag to the side quickly.
//need to create the barrier in the top that if a dropped fella remains in that line the game ends. 
//add background music
//add sound to: different sounds for different animais once they are merged into realit, 
//have to remove the shadows. <- to teste


//BUG
//the destroy all below is not working.
//also ui not appearing in the builded version.



//GOAL
///fix the queue list. make it smoothly move to the side but show only 3 of the options
///make the fox a bit rarer.
///put the ground, the top line in the right place in all shapes.
//create the mechanic for the line in the top ending the game.

//GOAL
///create the hyper moment. merging things fills a bar that once filled 
//create an ammo system for the power which you can use instead of watching an ad. <= teste
//creat ethe ad system and make you be able to call the power by watching it. <= teste
//create the system for the bonus to gift ammo for powers. <= teste


//PROBLEMS
///its not spending the ammo
///no effect for the spending or gaining.
///cancel button is not cancelling
///cancel button is not closing
///cancekl button should be dancing
///calling the ad is not calling the power.
///the evolved buttonn is not reacting to the target trigger.
///certain times below chicken is not dying.


//GOAL TOMORROW
///once the ball is over the line is starts a timer for the game to be lost.
///create death ui fopr when you lose. 
//start putting the sounds. find a new background music.



//PROBLEMS
///the heart animation is not working
///the confirm menu is not working. its just not animating.
///the queue image is all broken now. they keep appearing to the sides.
///and also you cannot change the position of drop



//GOAL TOMORROW
//create pause menu. leave some tips there to help the players. create a work around the floor being visibile during the pause
//save system to save the top score.
//settings button for remove sounds.
//save system to save the highest score. i wont use my system only the basic one in the thing. its really just one value.

//GOAL TOMORROW
//effect for the powers.
//improve the fox effects. like eating sound when it passes by an enemy.

//GOAL TOMORROW
//




//GOAL TOMORROW 
//create the farm
//you can clikc a button which will pause your run and then move to the side where you will see your farm




//NOTE
//the collider in the side should be a bit closer. it happens because of the screen. the thing they need to scale based in the screen.


//GOAL TOMORROW
//they need to roll more to the side when pushed.
//create system for improving a farm by completing achivements. the score you gain can buy stuff (like animals, buildings and clothes)


//i will try changing it to 2d


//EVERYTHING
//so basically we will open directly to the game. and there will be an option by the side to see your farm
//need to make the box smaller and made for 
//needd a better linerender. something with an animation perphaps
//i will create the especial balls: fox
//create the little bar showing all combinations
//powers: destroy a especifc fruit, destroy all fruit below x, upgraded a target fruit
//better physics:
//an effect when they merge together. like they both actually come together and there is a little explosion
//background music
//sound effects for: merge, collision, powers, button, especial commemorations for certain things
//ad system
//some popup fade texts saying "Good" "awesome"
//each time you play the game will record the score made in this play and award a bonus for it.




//IDEAS
//especial fruit that combines with everything.
//creature bomb.
//creature that eliminates all below x then touches the ground and disappears.


//CLARA 
//shadow
//the fox is unclear how it works. there should be something showing
//a ball got stuck in the screen and stopped interacting. it happened after a lot of small ones merge together.
//line of limit in the top.
//


//the groubnd is too up
//the topline is toio down
//their collider a bit too big and they seem to be floating
//there is a bug that for some reason the ball just get stuck in the air. and stops interacting.



//GOAL
///still the bug when two merge at the same time. i need to create a lock to interact with only once per turn.
///the exact score gained should always show. the congratulation should be higher in the y. when merging the balls.
///make an effect for the currentballmerge instead of it just popping into existence
///sometimes when you click play it just doesnt work
///create a warning to make it clear that you won something. right in the middle of the screen for a short duration
///create a dance effect for the thing you gain.
///create pause 
///can remove sound from the game. remove the background value or the sfx.

//CLARA
///the wipe power is not working
//the end screen not closing sometimes.
///if you dont get thegift and the thing dont care
//i want to upgrade only those below cat.
//create effects for fox
//create a tutorial for the mechanics.
//create a basic main menu
//need to make the game a bit harder. 
//need to fit the adbanner somewhere.
//create a puff effect for merging.
//solve the ad solution


//PROBLEMAS
///the ad has changed its way to build it. have to find new solution.


//SOUND
//bacgkround music
//wyhen the current ball spawns.
//certain merges have different sounds
//when got gift
//while in bonus
//when got to bonus
//




//TO FIX THIS NEW VERSION
//send it to github
///change the linerend to origiunal
///the first one is not appearing.
///new merge balls should not be transparent.
///fadeui is behind the things
//the powers confirmation are not working
//while you have the confirmation open you cannot input


//LAST IMPORTANT PART
///create cute effect for merge.
///fix the bar. use the bar as ref for a localpos instead of fix position.
///the pause ui is not scaling properly.
///instead of  aplay button i will have fadeout screen. that should be more than enough time for it to fix itself.
//need to inform the player somehow about the fox.


//CHECK EVERY PART OF THE GAME