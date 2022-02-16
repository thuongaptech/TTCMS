$(document).ready(function() { 

$("#topmenu ul li").hover(function(){ 

        $(this).find('ul:first').css({visibility: "visible",display: "none"}).show(400); 
        },function(){ 
        $(this).find('ul:first').css({visibility: "hidden"}); 

        }); 
});  
/*---------------------leftmenu----------------------------*/
$(document).ready(function() { 

$("#leftmenu ul li").hover(function(){ 

        $(this).find('ul:first').css({visibility: "visible",display: "none"}).show(400); 
        },function(){ 
        $(this).find('ul:first').css({visibility: "hidden"}); 

        }); 
});  

