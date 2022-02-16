﻿function ComponentArt_Rotator(){this.GlobalID="";this.ElementID="";this.ContainerID="";this.AutoStart=true;this.HideEffect=null;this.HideEffectDuration=0;this.Loop=true;this.PauseOnMouseOver=true;this.RotationType="ContentScroll";this.ScrollDirection="up";this.ScrollInterval=10;this.ScrollStep=1;this.ShowEffect=null;this.ShowEffectDuration=0;this.SlidePause=2000;this.SmoothScrollSpeed="Medium";this.Slides=new Array();this.Tickers=new Array();this.LeadTickers=new Array();this.CurrentSlide=-1;this.CurrentLeadTicker=0;this.ScrollIntervalID=0;this.NextSlideTimeoutID=0;this.HideTimeoutID=0;this.FirstTimeAround=true;this.HasTickers=false;this.FirstTicker=null;this.Stopped=false;this.Ticking=false;this.Status="";}ComponentArt_Rotator.prototype.GetProperty=function(_1){return this[_1];};ComponentArt_Rotator.prototype.SetProperty=function(_2,_3){this[_2]=_3;};ComponentArt_Rotator.prototype.Destroy=function(){window.clearInterval(this.ScrollIntervalID);rcr_Stop(this);};function rcr_Start(_4){if(cart_browser_ie){if(_4.RotationType=="SlideShow"){ss_ShowNextSlide(_4);}else{_z1B7(_4);if(_4.HasTickers){rcr_StartTickerSequence(_4);}else{var _5="scroll_ShowNextSlide("+_4.GlobalID+")";_4.NextSlideTimeoutID=window.setTimeout(_5,_4.SlidePause);}}}else{if(_4.RotationType=="SlideShow"){ss_ShowNextSlide(_4);}else{_z1B7(_4);scroll_ShowNextSlide(_4);}}}function _z1AA(_6){if(!_6.Stopped){return null;}_6.Stopped=false;if(!_6.Ticking){if(_6.RotationType=="SlideShow"){_z1BB(_6);}else{_z1B8(_6);}}}window.rcr_Stop=function(_7){if(_7.Stopped){return null;}_7.Stopped=true;window.clearTimeout(_7.NextSlideTimeoutID);window.clearTimeout(_7.HideTimeoutID);if(_7.SlidePause==0){window.clearInterval(_7.ScrollIntervalID);}if(_7.RotationType=="SlideShow"){var _8=document.getElementById(_7.ContainerID);_8.style.visibility="visible";}};function _z1AD(_9){if(cart_browser_ie){if(_9.CurrentSlide==-1){if(_9.RotationType=="SlideShow"){_9.CurrentSlide=0;}else{_9.CurrentSlide=1;}}else{if(_9.CurrentSlide==_9.Slides.length-1){_9.CurrentSlide=0;_9.FirstTimeAround=false;}else{_9.CurrentSlide++;}}}else{if(_9.CurrentSlide==-1){_9.CurrentSlide=0;}else{if(_9.CurrentSlide==_9.Slides.length-1){_9.CurrentSlide=0;_9.FirstTimeAround=false;}else{_9.CurrentSlide++;}}}}function _z1B7(_a){if(cart_browser_ie){var _b=document.getElementById(_a.ContainerID);_b.style.visibility="visible";_b.parentNode.scrollTop=0;_b.parentNode.scrollLeft=0;}else{var _b=document.getElementById(_a.ContainerID);var _c=document.getElementById(_a.ElementID);var _d;var _e;switch(_a.ScrollDirection){case "up":_d=parseInt(_c.style.height.replace("px",""))+"px";_e="0px";break;case "left":_d="0px";_e=parseInt(_c.style.width.replace("px",""))+"px";break;}_b.style.top=_d;_b.style.left=_e;_b.style.visibility="visible";}}function _z1B8(_f){if(_f.ScrollIntervalID==0){scroll_ShowNextSlide(_f);}else{if(_f.SlidePause==0){var _10="scroll_NextSlideToView("+_f.GlobalID+")";_f.ScrollIntervalID=window.setInterval(_10,_f.ScrollInterval);}}}function scroll_ShowNextSlide(_11){_z1AD(_11);if(!_11.Loop&&!_11.FirstTimeAround){rcr_Stop(_11);return null;}var _12="scroll_NextSlideToView("+_11.GlobalID+")";_11.ScrollIntervalID=window.setInterval(_12,_11.ScrollInterval);}function scroll_NextSlideToView(_13){if(cart_browser_ie){var _14=document.getElementById(_13.ContainerID);var _15=document.getElementById(_13.Slides[_13.CurrentSlide]);var _16=_14.parentNode.scrollTop;var _17=_14.parentNode.scrollLeft;var _18=document.getElementById(_13.Slides[_13.CurrentSlide]);if(_18){var _19=_18.offsetHeight;var _1a=_18.offsetWidth;}var _1b=0;switch(_13.ScrollDirection){case "up":if(_13.RotationType=="ContentScroll"){_16+=_13.ScrollStep;}else{_1b=abs(_19-_16)/_z1A7(_13);if(_1b<=2){_1b=1;}_16+=_1b;}break;case "left":if(_13.RotationType=="ContentScroll"){_17+=_13.ScrollStep;}else{_1b=abs(_1a-_17)/_z1A7(_13);if(_1b<=2){_1b=1;}_17+=_1b;}break;}if((_16>=_19&&_13.ScrollDirection=="up")||(_17>=_1a&&_13.ScrollDirection=="left")){window.clearInterval(_13.ScrollIntervalID);_13.ScrollIntervalID=0;if(!(_13.FirstTimeAround&&_13.CurrentSlide==0)){_z1BA(_13);}_14.parentNode.scrollTop=0;_14.parentNode.scrollLeft=0;if(_13.HasTickers){rcr_StartTickerSequence(_13);}else{var _1c="scroll_ShowNextSlide("+_13.GlobalID+")";if(!_13.Stopped){_13.NextSlideTimeoutID=window.setTimeout(_1c,_13.SlidePause);}}}else{_14.parentNode.scrollTop=_16;_14.parentNode.scrollLeft=_17;}}else{var _14=document.getElementById(_13.ContainerID);var _15=document.getElementById(_13.Slides[_13.CurrentSlide]);var _16=parseInt(_14.style.top.replace("px",""));var _17=parseInt(_14.style.left.replace("px",""));var _1d=0;var _1e=0;var _1f=document.getElementById(_13.Slides[_z1B9(_13)]);if(!(_13.FirstTimeAround&&_13.CurrentSlide==0)){_1d=_1f.offsetHeight;_1e=_1f.offsetWidth;}var _1b=0;switch(_13.ScrollDirection){case "up":if(_13.RotationType=="ContentScroll"){_16-=_13.ScrollStep;}else{_1b=abs(_1d+_16)/_z1A7(_13);if(_1b<=2){_1b=1;}_16-=_1b;}break;case "left":if(_13.RotationType=="ContentScroll"){_17-=_13.ScrollStep;}else{_1b=abs(_1e+_17)/_z1A7(_13);if(_1b<=2){_1b=1;}_17-=_1b;}break;}_14.style.top=_16+"px";_14.style.left=_17+"px";if((_16+_1d==0&&_13.ScrollDirection=="up")||(_17+_1e==0&&_13.ScrollDirection=="left")){window.clearInterval(_13.ScrollIntervalID);_13.ScrollIntervalID=0;if(!(_13.FirstTimeAround&&_13.CurrentSlide==0)){_z1BA(_13);}if(_13.HasTickers){rcr_StartTickerSequence(_13);}else{var _1c="scroll_ShowNextSlide("+_13.GlobalID+")";if(!_13.Stopped){_13.NextSlideTimeoutID=window.setTimeout(_1c,_13.SlidePause);}}}}}function _z1BA(_20){if(cart_browser_ie){var _21=document.getElementById(_20.ContainerID);if(_20.ScrollDirection=="up"){var _22=document.getElementById(_20.Slides[_z1B9(_20)]);var _23=_22.cloneNode(true);_21.removeChild(_22);_21.parentNode.scrollTop=0;_21.appendChild(_23);_z1AB(_20);}else{var _24=document.getElementById(_20.ContainerRowID);var _25=_24.cells[0];var a=_24.removeChild(_25);_21.parentNode.scrollLeft=0;var b=_24.appendChild(a);_z1AB(_20);}}else{var _21=document.getElementById(_20.ContainerID);if(_20.ScrollDirection=="up"){var _22=document.getElementById(_20.Slides[_z1B9(_20)]);var _23=_22.cloneNode(true);_21.removeChild(_22);_21.style.top="0px";_21.appendChild(_23);_z1AB(_20);}else{var _24=document.getElementById(_20.ContainerRowID);var _25=_24.cells[0];var a=_24.removeChild(_25);_21.style.left="0px";var b=_24.appendChild(a);_z1AB(_20);}}}function _z1B9(_28){if(_28.CurrentSlide==0){return _28.Slides.length-1;}else{return _28.CurrentSlide-1;}}function _z1A7(_29){switch(_29.SmoothScrollSpeed){case "Slow":return 8;break;case "Medium":return 6;break;case "Fast":return 4;break;}}function _z1BB(_2a){if(_2a.HasTickers&&_2a.Status=="PlayingShowEffect"){return null;}if(!_2a.Ticking){ss_PlayHideEffect(_2a);var _2b=0;if(_2a.HideEffect){_2b=_2a.HideEffectDuration;}functionParam="ss_ShowNextSlide("+_2a.GlobalID+")";_2a.NextSlideTimeoutID=window.setTimeout(functionParam,_2b);}}function ss_ShowNextSlide(_2c){if(_2c.Stopped){return null;}_z1AD(_2c);var _2d=document.getElementById(_2c.ContainerID);var _2e=document.getElementById(_2c.Slides[_2c.CurrentSlide]);_2d.innerHTML=_2e.innerHTML;_2e.innerHTML="";_z1AB(_2c);_z1BC(_2c);if(_2c.HasTickers){var _2f="rcr_StartTickerSequence("+_2c.GlobalID+")";var _30=window.setTimeout(_2f,_2c.ShowEffectDuration);}else{var _2f="ss_DisplaySlide("+_2c.GlobalID+")";_2c.NextSlideTimeoutID=window.setTimeout(_2f,_2c.ShowEffectDuration);}}function ss_DisplaySlide(_31){if(_31.Stopped){return null;}_31.Status="DisplayingSlide";window.clearTimeout(_31.HideTimeoutID);window.clearTimeout(_31.NextSlideTimeoutID);if(!_31.Loop&&_31.CurrentSlide==_31.Slides.length-1){rcr_Stop(_31);return null;}var _32="ss_PlayHideEffect("+_31.GlobalID+")";_31.HideTimeoutID=window.setTimeout(_32,_31.SlidePause);var _33=0;if(_31.HideEffect){_33+=_31.HideEffectDuration;}_33+=_31.SlidePause;_32="ss_ShowNextSlide("+_31.GlobalID+")";_31.NextSlideTimeoutID=window.setTimeout(_32,_33);}function _z1BC(_34){_34.Status="PlayingShowEffect";var _35=document.getElementById(_34.ContainerID);if(_35.filters&&_34.ShowEffect){_35.style.filter=_34.ShowEffect;_35.filters[0].apply();}_35.style.visibility="visible";if(_35.filters&&_34.ShowEffect){_35.filters[0].play();}}function ss_PlayHideEffect(_36){_36.Status="PlayingHideEffect";var _37=document.getElementById(_36.ContainerID);if(_37.filters&&_36.HideEffect){_37.style.filter=_36.HideEffect;_37.filters[0].apply();}var _38=document.getElementById(_36.Slides[_36.CurrentSlide]);_38.innerHTML=_37.innerHTML;_37.style.visibility="hidden";if(_37.filters&&_36.HideEffect){_37.filters[0].play();}}function rcr_StartTickerSequence(_39){_39.Status="RunningTickers";_39.Ticking=true;rcr_StartTicker(_39.LeadTickers[_39.CurrentLeadTicker]);}function rcr_EndTickerSequence(_3a){_3a.Ticking=false;if(!_3a.Stopped){if(_3a.RotationType=="SlideShow"){ss_DisplaySlide(_3a);}else{var _3b="scroll_ShowNextSlide("+_3a.GlobalID+")";_3a.NextSlideTimeoutID=window.setTimeout(_3b,_3a.SlidePause);}}_z1AC(_3a);}function _z1AC(_3c){if(_3c.CurrentLeadTicker==_3c.LeadTickers.length-1){_3c.CurrentLeadTicker=0;}else{_3c.CurrentLeadTicker++;}}function _z1AB(_3d){if(_3d.HasTickers){for(var i=0;i<_3d.Tickers.length;i++){_z1AE(_3d.Tickers[i],"");}}}function ie_MsOver(obj,_40){if(!obj.contains(event.fromElement)&&_40){rcr_Stop(_40);}}function ie_MsOut(obj,_42){if(!obj.contains(event.toElement)&&_42){_z1AA(_42);}}function ns_MsOver(evt,_44,_45){if(_z1A3(_44,evt)&&_45){rcr_Stop(_45);}}function ns_MsOut(evt,_47,_48){if(!_z1A3(_47,evt)&&_48){_z1AA(_48);}}function _z1A3(_49,evt){if(_49!=null){var obj=document.getElementById(_49);var _4c=_z1A1(obj)-1;var _4d=_z1A2(obj)-1;var _4e=_4c+obj.offsetWidth+1;var _4f=_4d+obj.offsetHeight+1;if((evt.pageX>_4c)&&(evt.pageX<_4e)&&(evt.pageY>_4d)&&(evt.pageY<_4f)){return true;}else{return false;}}else{return false;}}function _z1A1(_50){var x=0;do{if(_50.style.position=="absolute"){return x+_50.offsetLeft;}else{x+=_50.offsetLeft;if(_50.offsetParent){if(_50.offsetParent.tagName=="TABLE"){if(parseInt(_50.offsetParent.border)>0){x+=1;}}}}}while((_50=_50.offsetParent));return x;}function _z1A2(_52){var y=0;do{if(_52.style.position=="absolute"){return y+_52.offsetTop;}else{y+=_52.offsetTop;if(_52.offsetParent){if(_52.offsetParent.tagName=="TABLE"){if(parseInt(_52.offsetParent.border)>0){y+=1;}}}}}while((_52=_52.offsetParent));return y;}function abs(x){if(x<0){return -x;}else{return x;}}

if(typeof(Sys)!=='undefined')Sys.Application.notifyScriptLoaded();