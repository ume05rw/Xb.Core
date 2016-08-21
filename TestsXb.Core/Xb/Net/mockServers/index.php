<?php

$method = $_SERVER["REQUEST_METHOD"];
$_PUT = [];
$_DELETE = [];

$headers = [];
foreach (getallheaders() as $name => $value) {
	$headers[$name] = $value;
}

$body = file_get_contents('php://input');

$input_encode = "ASCII";
foreach ($_GET as $get_value) {
	$tmp_encode = mb_detect_encoding($get_value);
	if($tmp_encode != $input_encode)
	{
		$input_encode = $tmp_encode;
		break;
	}
}
if ($input_encode == "ASCII")
{
	$input_encode = mb_detect_encoding($body);
}

$all_params = json_encode($_GET).$body;
$input_encode = mb_detect_encoding($all_params);
$all_params = mb_convert_encoding($all_params, "UTF-8", $input_encode);

$output_encode = "UTF-8";
if (strpos($all_params, "utf8") != false) {
	$output_encode = "UTF-8";
} elseif (strpos($all_params, "sjis") != false) {
	$output_encode = "SJIS";
} elseif (strpos($all_params, "eucjp") != false) {
	$output_encode = "EUC-JP";
}

foreach ($_GET as $key => $val) {
	$tmp_encode = mb_detect_encoding($val);
	if($tmp_encode != "ASCII")
	{
		$_GET[$key] = mb_convert_encoding($val, "UTF-8", $tmp_encode);
	}
}

if ($method == "POST") {
	foreach ($_POST as $key => $val) {
		$tmp_encode = mb_detect_encoding($val);
		if($tmp_encode != "ASCII")
		{
			$_POST[$key] = mb_convert_encoding($val, "UTF-8", $tmp_encode);
		}
	}
} elseif ($method == "PUT") {
	parse_str($body, $_PUT);

	foreach ($_PUT as $key => $val) {
		$tmp_encode = mb_detect_encoding($val);
		if($tmp_encode != "ASCII")
		{
			$_PUT[$key] = mb_convert_encoding($val, "UTF-8", $tmp_encode);
		}
	}
} elseif ($method == "DELETE") {
	parse_str($body, $_DELETE);

	foreach ($_DELETE as $key => $val) {
		$tmp_encode = mb_detect_encoding($val);
		if($tmp_encode != "ASCII")
		{
			$_DELETE[$key] = mb_convert_encoding($val, "UTF-8", $tmp_encode);
		}
	}
}


$result = [
	'method' => $method,
	'headers' => $headers,
	'body' => $body,
	'passing_values' => array_merge($_GET, $_POST, $_PUT, $_DELETE),
	'url' => (empty($_SERVER["HTTPS"]) ? "http://" : "https://").$_SERVER["HTTP_HOST"].$_SERVER["REQUEST_URI"],
	'input_encode' => $input_encode,
	'output_encode' => $output_encode
];

if(array_key_exists("wait", $_GET)
   && is_int($_GET["wait"]))
{
	sleep($_GET["wait"]);
}

echo mb_convert_encoding(json_encode($result), $output_encode, "auto");
