if(typeof dd_domreadycheck=="undefined")
var dd_domreadycheck=false
var anylinkmenu={menusmap:{},preloadimages:[],effects:{delayhide:400,shadow:{enabled:true,opacity:0.3,depth:[5,5]},fade:{enabled:true,duration:500}},dimensions:{},ismobile:navigator.userAgent.match(/(iPad)|(iPhone)|(iPod)|(android)|(webOS)/i)!=null,getoffset:function(what,offsettype){return(what.offsetParent)?what[offsettype]+this.getoffset(what.offsetParent,offsettype):what[offsettype]},getoffsetof:function(el){el._offsets={left:this.getoffset(el,"offsetLeft"),top:this.getoffset(el,"offsetTop"),h:el.offsetHeight}},getdimensions:function(menu){this.dimensions={anchorw:menu.anchorobj.offsetWidth,anchorh:menu.anchorobj.offsetHeight,docwidth:(window.innerWidth||this.standardbody.clientWidth)-20,docheight:(window.innerHeight||this.standardbody.clientHeight)-15,docscrollx:window.pageXOffset||this.standardbody.scrollLeft,docscrolly:window.pageYOffset||this.standardbody.scrollTop}
if(!this.dimensions.dropmenuw){this.dimensions.dropmenuw=menu.dropmenu.offsetWidth
this.dimensions.dropmenuh=menu.dropmenu.offsetHeight}},isContained:function(m,e){var e=window.event||e
var c=e.relatedTarget||((e.type=="mouseover")?e.fromElement:e.toElement)
while(c&&c!=m)try{c=c.parentNode}catch(e){c=m}
if(c==m)
return true
else
return false},setopacity:function(el,value){el.style.opacity=value
if(typeof el.style.opacity!="string"){el.style.MozOpacity=value
if(document.all&&typeof el.style.filter=="string"){el.style.filter="progid:DXImageTransform.Microsoft.alpha(opacity="+value*100+")"}}},showmenu:function(menuid){var menu=anylinkmenu.menusmap[menuid]
clearTimeout(menu.hidetimer)
this.getoffsetof(menu.anchorobj)
this.getdimensions(menu)
var posx=menu.anchorobj._offsets.left+(menu.orientation=="lr"?this.dimensions.anchorw:0)
var posy=menu.anchorobj._offsets.top+this.dimensions.anchorh-(menu.orientation=="lr"?this.dimensions.anchorh:0)
if(posx+this.dimensions.dropmenuw+this.effects.shadow.depth[0]>this.dimensions.docscrollx+this.dimensions.docwidth){posx=posx-this.dimensions.dropmenuw+(menu.orientation=="lr"?-this.dimensions.anchorw:this.dimensions.anchorw)}
if(posy+this.dimensions.dropmenuh>this.dimensions.docscrolly+this.dimensions.docheight){posy=Math.max(posy-this.dimensions.dropmenuh-(menu.orientation=="lr"?-this.dimensions.anchorh:this.dimensions.anchorh),this.dimensions.docscrolly)}
if(this.effects.fade.enabled){this.setopacity(menu.dropmenu,0)
if(this.effects.shadow.enabled)
this.setopacity(menu.shadow,0)}
menu.dropmenu.setcss({left:posx+'px',top:posy+'px',visibility:'visible'})
if(this.effects.shadow.enabled){menu.shadow.setcss({left:posx+anylinkmenu.effects.shadow.depth[0]+'px',top:posy+anylinkmenu.effects.shadow.depth[1]+'px',visibility:'visible'})}
if(this.effects.fade.enabled){clearInterval(menu.animatetimer)
menu.curanimatedegree=0
menu.starttime=new Date().getTime()
menu.animatetimer=setInterval(function(){anylinkmenu.revealmenu(menuid)},20)}},revealmenu:function(menuid){var menu=anylinkmenu.menusmap[menuid]
var elapsed=new Date().getTime()-menu.starttime
if(elapsed<this.effects.fade.duration){this.setopacity(menu.dropmenu,menu.curanimatedegree)
if(this.effects.shadow.enabled)
this.setopacity(menu.shadow,menu.curanimatedegree*this.effects.shadow.opacity)}
else{clearInterval(menu.animatetimer)
this.setopacity(menu.dropmenu,1)
menu.dropmenu.style.filter=""}
menu.curanimatedegree=(1-Math.cos((elapsed/this.effects.fade.duration)*Math.PI))/2},setcss:function(param){for(prop in param){this.style[prop]=param[prop]}},setcssclass:function(el,targetclass,action){var needle=new RegExp("(^|\\s+)"+targetclass+"($|\\s+)","ig")
if(action=="check")
return needle.test(el.className)
else if(action=="remove")
el.className=el.className.replace(needle,"")
else if(action=="add"&&!needle.test(el.className))
el.className+=" "+targetclass},hidemenu:function(menuid){var menu=anylinkmenu.menusmap[menuid]
clearInterval(menu.animatetimer)
menu.dropmenu.setcss({visibility:'hidden',left:0,top:0})
menu.shadow.setcss({visibility:'hidden',left:0,top:0})},getElementsByClass:function(targetclass){if(document.querySelectorAll)
return document.querySelectorAll("."+targetclass)
else{var classnameRE=new RegExp("(^|\\s+)"+targetclass+"($|\\s+)","i")
var pieces=[]
var alltags=document.all?document.all:document.getElementsByTagName("*")
for(var i=0;i<alltags.length;i++){if(typeof alltags[i].className=="string"&&alltags[i].className.search(classnameRE)!=-1)
pieces[pieces.length]=alltags[i]}
return pieces}},addDiv:function(divid,divclass,inlinestyle){var el=document.createElement("div")
if(divid)
el.id=divid
el.className=divclass
if(inlinestyle!=""&&typeof el.style.cssText=="string")
el.style.cssText=inlinestyle
else if(inlinestyle!="")
el.setAttribute('style',inlinestyle)
document.body.appendChild(el)
return el},getmenuHTML:function(menuobj){var menucontent=[]
var frag=""
for(var i=0;i<menuobj.items.length;i++){frag+='<li><a href="'+menuobj.items[i][1]+'" target="'+menuobj.linktarget+'">'+menuobj.items[i][0]+'</a></li>\n'
if(menuobj.items[i][2]=="efc"||i==menuobj.items.length-1){menucontent.push(frag)
frag=""}}
if(typeof menuobj.cols=="undefined")
return '<ul>\n'+menucontent.join('')+'\n</ul>'
else{frag=""
for(var i=0;i<menucontent.length;i++){frag+='<div class="'+menuobj.cols.divclass+'" style="'+menuobj.cols.inlinestyle+'">\n<ul>\n'+menucontent[i]+'</ul>\n</div>\n'}
return frag}},addEvent:function(targetarr,functionref,tasktype){if(targetarr.length>0){var target=targetarr.shift()
if(target.addEventListener)
target.addEventListener(tasktype,functionref,false)
else if(target.attachEvent)
target.attachEvent('on'+tasktype,function(){return functionref.call(target,window.event)})
this.addEvent(targetarr,functionref,tasktype)}},domready:function(functionref){if(dd_domreadycheck){functionref()
return}
if(document.addEventListener){document.addEventListener("DOMContentLoaded",function(){document.removeEventListener("DOMContentLoaded",arguments.callee,false)
functionref();dd_domreadycheck=true},false)}
else if(document.attachEvent){if(document.documentElement.doScroll&&window==window.top)(function(){if(dd_domreadycheck)return
try{document.documentElement.doScroll("left")}catch(error){setTimeout(arguments.callee,0)
return;}
functionref();dd_domreadycheck=true})();}
if(document.attachEvent&&parent.length>0)
this.addEvent([window],function(){functionref()},"load");},addState:function(anchorobj,state){if(anchorobj.getAttribute('data-image')){var imgobj=(anchorobj.tagName=="IMG")?anchorobj:anchorobj.getElementsByTagName('img')[0]
if(imgobj){imgobj.src=(state=="add")?anchorobj.getAttribute('data-overimage'):anchorobj.getAttribute('data-image')}}
else
anylinkmenu.setcssclass(anchorobj,"selectedanchor",state)},addState:function(anchorobj,state){if(anchorobj.getAttribute('data-image')){var imgobj=(anchorobj.tagName=="IMG")?anchorobj:anchorobj.getElementsByTagName('img')[0]
if(imgobj){imgobj.src=(state=="add")?anchorobj.getAttribute('data-overimage'):anchorobj.getAttribute('data-image')}}
else
anylinkmenu.setcssclass(anchorobj,"selectedanchor",state)},setupmenu:function(targetclass,anchorobj,pos){this.standardbody=(document.compatMode=="CSS1Compat")?document.documentElement:document.body
var relattr=anchorobj.getAttribute("rel")
dropmenuid=relattr.replace(/\[(\w+)\]/,'')
var dropmenuvar=window[dropmenuid]
var dropmenu=this.addDiv(null,dropmenuvar.divclass,dropmenuvar.inlinestyle)
dropmenu.innerHTML=this.getmenuHTML(dropmenuvar)
var menu=this.menusmap[targetclass+pos]={id:targetclass+pos,anchorobj:anchorobj,dropmenu:dropmenu,revealtype:(relattr.length!=dropmenuid.length&&RegExp.$1=="click")||anylinkmenu.ismobile?"click":"mouseover",orientation:anchorobj.getAttribute("rev")=="lr"?"lr":"ud",shadow:this.addDiv(null,"anylinkshadow",null)}
menu.anchorobj._internalID=targetclass+pos
menu.anchorobj._isanchor=true
menu.dropmenu._internalID=targetclass+pos
menu.shadow._internalID=targetclass+pos
menu.dropmenu.setcss=this.setcss
menu.shadow.setcss=this.setcss
menu.shadow.setcss({width:menu.dropmenu.offsetWidth+"px",height:menu.dropmenu.offsetHeight+"px"})
this.setopacity(menu.shadow,this.effects.shadow.opacity)
this.addEvent([menu.anchorobj,menu.dropmenu,menu.shadow],function(e){var menu=anylinkmenu.menusmap[this._internalID]
if(this._isanchor&&menu.revealtype=="mouseover"&&!anylinkmenu.isContained(this,e)){anylinkmenu.showmenu(menu.id)
anylinkmenu.addState(this,"add")}
else if(typeof this._isanchor=="undefined"){clearTimeout(menu.hidetimer)}},"mouseover")
this.addEvent([menu.anchorobj,menu.dropmenu,menu.shadow],function(e){if(!anylinkmenu.isContained(this,e)){var menu=anylinkmenu.menusmap[this._internalID]
menu.hidetimer=setTimeout(function(){anylinkmenu.addState(menu.anchorobj,"remove")
anylinkmenu.hidemenu(menu.id)},anylinkmenu.effects.delayhide)}},"mouseout")
this.addEvent([menu.anchorobj,menu.dropmenu],function(e){var menu=anylinkmenu.menusmap[this._internalID]
if(this._isanchor&&menu.revealtype=="click"){if(menu.dropmenu.style.visibility=="visible")
anylinkmenu.hidemenu(menu.id)
else{anylinkmenu.addState(this,"add")
anylinkmenu.showmenu(menu.id)}
if(e.preventDefault)
e.preventDefault()
return false}
else
menu.hidetimer=setTimeout(function(){anylinkmenu.hidemenu(menu.id)},anylinkmenu.effects.delayhide)},"click")},init:function(targetclass){this.domready(function(){anylinkmenu.trueinit(targetclass)})},trueinit:function(targetclass){var anchors=this.getElementsByClass(targetclass)
var preloadimages=this.preloadimages
for(var i=0;i<anchors.length;i++){if(anchors[i].getAttribute('data-image')){preloadimages[preloadimages.length]=new Image()
preloadimages[preloadimages.length-1].src=anchors[i].getAttribute('data-image')}
if(anchors[i].getAttribute('data-overimage')){preloadimages[preloadimages.length]=new Image()
preloadimages[preloadimages.length-1].src=anchors[i].getAttribute('data-overimage')}
this.setupmenu(targetclass,anchors[i],i)}}}