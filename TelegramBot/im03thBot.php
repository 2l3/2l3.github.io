<?php

ob_start();
define('API_KEY','2088538503:AAEsfDgx4Ulm-7_bXh0WG99px61qddn-6gI');
function bot($method,$datas=[]){
    $url = "https://api.telegram.org/bot".API_KEY."/".$method;
$ch = curl_init();
    curl_setopt($ch,CURLOPT_URL,$url);
    curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
    curl_setopt($ch,CURLOPT_POSTFIELDS,$datas);
    $res = curl_exec($ch);
    if(curl_error($ch)){
        var_dump(curl_error($ch));
    }else{
        return json_decode($res);
    }
}

$update = json_decode(file_get_contents('php://input'));
$message = $update->message;
$chat_id = $message->chat->id;
$text = $message->text;
$chat_id2 = $update->callback_query->message->chat->id;
$message_id = $update->callback_query->message->message_id;
$data = $update->callback_query->data;

if($text=="/start"){
	bot('sendmessage',[
	'chat_id'=>$chat_id,
	'text'=>"ياهلا ومرحبا",
	]);
	}

if($text=="هلا"){
	bot('sendmessage',[
	'chat_id'=>$chat_id,
	'text'=>"اخلص وش تبي",
	]);
	}


if($text=="زق"){
	bot('sendmessage',[
	'chat_id'=>$chat_id,
	'text'=>"حذفت رسالتك يالوصخ",
	]);
	bot('deletemessage',[
	'chat_id'=>$chat_id,
	'message_id'=> $message
	          -> message_id
	]);
	}
