var colors = ['#FF0000','#00FF00','#0000FF'], color = 0, delay = 5000, scrollDelay = 1000;


function getCurrentStatus() {
    $.get(statusAddress, function (data) {
        $('#status').html(data);
    });
}

function getCurrentSummary() {
    $.get(summaryAddress, function (data) {
        $('#summary').html(data);
    });
}

//var $burnGuard = $('<div>').attr('id','burnGuard').css({
//    'background-color':'#FF00FF',
//    'width':'1px',
//    'height':$(document).height()+'px',
//    'position':'absolute',
//    'top':'0px',
//    'left':'0px',
//    'display':'none'
//}).appendTo('body');

function burnGuardAnimate()
{
    color = ++color % 3;
    var rColor = colors[color];
    $burnGuard.css({
        'left':'0px',
        'background-color':rColor}).show().animate({
        'left':$(window).width()+'px'
    },scrollDelay,function(){
        $(this).hide();
    });
    setTimeout(burnGuardAnimate,delay);
}


$.ajaxSetup({ timeout: 10000 });
setInterval("getCurrentStatus()", 10000);
setInterval("getCurrentSummary()", 60000);
//setTimeout(burnGuardAnimate,delay);
