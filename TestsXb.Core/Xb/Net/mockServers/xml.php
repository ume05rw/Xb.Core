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


$xml = simplexml_load_string($body);
$passingValues = [];
if($xml !== FALSE) {
	foreach ($xml as $key => $val) {
		$tmp_encode = mb_detect_encoding($val);
		if($tmp_encode != "ASCII")
		{
			$passingValues[$key] = mb_convert_encoding($val, "UTF-8", $tmp_encode);
		}
		else
		{
			$passingValues[$key] = $val;
		}
	}
}

$headerList = [];
foreach($headers as $key => $val) {
	$headerList[] = $key."-=-=DLMT1=-=-".$val;
}

$passingList = [];
foreach(array_merge($_GET, $passingValues) as $key => $val) {
	$passingList[] = $key."-=-=DLMT1=-=-".$val;
}

$result = [
	'method' => $method,
	'headers' => implode("-=-=DLMT2=-=-", $headerList),
	'body' => $body,
	'passing_values' => implode("-=-=DLMT2=-=-", $passingList),
	'url' => (empty($_SERVER["HTTPS"]) ? "http://" : "https://").$_SERVER["HTTP_HOST"].$_SERVER["REQUEST_URI"],
	'input_encode' => $input_encode,
	'output_encode' => $output_encode
];

if(array_key_exists("wait", $_GET)
   && is_int($_GET["wait"]))
{
	sleep($_GET["wait"]);
}

$xml_data = new SimpleXMLElement('<?xml version="1.0"?><HttpResultType></HttpResultType>');
array_to_xml($result, $xml_data);

echo mb_convert_encoding($xml_data->asXML(), $output_encode, "auto");


// http://stackoverflow.com/questions/1397036/how-to-convert-array-to-simplexml
// function defination to convert array to xml
function array_to_xml( $data, &$xml_data ) {
    foreach( $data as $key => $value ) {
        if( is_array($value) ) {
            if( is_numeric($key) ){
                $key = 'item'.$key; //dealing with <0/>..<n/> issues
            }
            $subnode = $xml_data->addChild($key);
            array_to_xml($value, $subnode);
        } else {
            $xml_data->addChild("$key",htmlspecialchars("$value"));
        }
     }
}

