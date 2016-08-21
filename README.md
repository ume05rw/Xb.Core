Xb.Core
====

Xamarin Ready, Simple Library(PCL) for .Net Framework 4.5 or later.

## Description
It's Simple Lybrary, most of these are the static method.
Functions for Bytes, Date, Number, String, Http-Query and Console.

## Requirement
No Requirement.

## Usage
1. Add reference [Xb.Core.dll](https://github.com/ume05rw/Xb.Core/blob/master/binary/Xb.Core.dll) to your project.
2. Call Static Methods Xb.Any(), or Create Instance.

Namespace and Methods are...

    ・Xb.Net
         |
         +- .Http(Instance)
         |    |
         |    +- .Constructor(string url,
         |    |               string passingValues = null,
         |    |               Xb.Net.Http.MethodType method = Xb.Net.Http.MethodType.Get,
         |    |               Dictionary<HttpRequestHeader, string> headers = null)
         |    |   Create Xb.Net.Http Instance
         |    |
         |    +- .GetResponseAsync()
         |    |   Get WebResponse and Stream by url
         |    |
         |    +- .GetAsync()
         |        Get String by url
         |
         +- .Http(Static)
         |    |
         |    +- .GetParamString(Dictionary<string, object> values)
         |    |   Convert Associative-Array to Http-Parameter-String
         |    |
         |    +- .EncodeUri(string text)
         |    |   Get Uri-Encode string
         |    |
         |    +- .DecodeUri(string text)
         |    |   Get Uri-Decode string
         |    |
         |    +- .GetFilename(string url)
         |    |   Get File Name from url
         |    |
         |    +- .GetDirectory(string url)
         |    |   Get Directory Name from url
         |    |
         |    +- .IsValidUrl(string url)
         |        Validate url string
         |
         +- .HttpXml(Instance)
         |    |
         |    +- .Constructor(string url,
         |    |               Dictionary<string, object> passingValues = null,
         |    |               Xb.Net.Http.MethodType method = Xb.Net.Http.MethodType.Post,
         |    |               Dictionary<HttpRequestHeader, string> headers = null)
         |    |   Create Xb.Net.HttpXml Instance
         |    |
         |    +- .GetResponseAsync()
         |    |   Get WebResponse and Stream by url
         |    |
         |    +- .GetAsync<T>()
         |        Get Response from url, and Cast response to <Class you've defined.>
         |
         +- .HttpXml(Static)
              |
              +- .GetParamString(Dictionary<string, object> values)
                  Convert Associative-Array to Http-Parameter-String
    
    ・Xb.Byte
         |
         +- .GetBase64String(byte[] bytes)
         |   Get Base64-String from Byte Array
         |
         +- .GetBase64String(System.IO.Stream stream)
         |   Get Base64-String from Byte Array
         |
         +- .GetBytes(System.IO.Stream stream)
         |   Get Byte Array from Stream
         |
         +- .GetStream(byte[] bytes)
         |   Get Stream from Byte Array
         |
         +- .GetStream(string base64String)
         |   Get Stream from Base64-String
         |
         +- .GetBitString(byte value)
         |   Get Bit-String from 1byte
         |
         +- .GetBitString(int value)
         |   Get Bit-String from integer(regarded as unsigned)
         |
         +- .ByteArray(Instance)
              |
              +- .Constructor(byte[] bytes)
              |   Create Xb.Byte.ByteArray Instance
              |
              +- .GetBit(int byteIndex, int bitIndex)
              |   Get Bit value
              |
              +- .SetBit(int byteIndex, int bitIndex, bool value)
              |   Write Bit value
              |
              +- .GetInteger(int byteIndex, int length)
              |   Get Integer from byte range
              |
              +- .GetLong(int byteIndex, int length)
                  Get Long from byte range
    
    ・Xb.Date
         |
         +- .GetDate(string dateTimeString = null)
         |   Get DateTime from String
         |
         +- .GetDate(long unixTime, bool isMilliSec = false)
         |   Get DateTime from Unix-Time integer
         |
         +- .GetUnixtime(DateTime dateTime, bool isMilliSec = false)
         |   Get Unix-Time integer
         |
         +- .GetLastDate(DateTime dateTime)
         |   Get last-date in month
         |
         +- .GetLastDate(string yyyy, string mm)
         |   Get last-date in month
         |
         +- .GetLastDate(int yyyy, int mm)
         |   Get last-date in month
         |
         +- .GetLastDay(DateTime dateTime)
         |   Get last-day integer in month
         |
         +- .GetLastDay(DateTime dateTime)
         |   Get last-day integer in month
         |
         +- .GetLastDay(string yyyy, string mm)
         |   Get last-day integer in month
         |
         +- .GetLastDay(int yyyy, int mm)
         |   Get last-day integer in month
         |
         +- .IsDate(string value)
         |   Validate DateTime-String
         |
         +- .IsTime(string value)
         |   Validate Time-String
         |
         +- .FormatDb(DateTime dateTime)
         |   Get Database-DateTime String
         |
         +- .FormatString(string dateString, 
         |                string dateSplitter = "-", 
         |                string timeSplitter = ":", 
         |                string spacer = " ")
         |   Format DateTime String
         |
         +- .GetTimestampString(DateTime targetDateTime, DateTime? afterDateTime = null)
             Get Update-Time sring
          
    ・Xb.Num
         |
         +- .Round(decimal value, 
         |         RoundType roundType = RoundType.HalfUp, 
         |         int decimalPoint = 0)
         |   Get Rounded Number
         |
         +- .IsNumeric(string value)
             Validate Number-String
    
    ・Xb.Str
         |
         +- .Left(string target, int length)
         |   Get substring left side
         |
         +- .Right(string target, int length)
         |   Get substring right side
         |
         +- .Slice(string target, int length)
         |   Get sliced substring
         |
         +- .SliceReverse(string target, int length)
         |   Get cutted substring
         |
         +- .LeftSentence(string target, int length, string delimiter = "/")
         |   Get sliced string block left side
         |
         +- .RightSentence(string target, int length, string delimiter = "/")
         |   Get sliced string block right side
         |
         +- .SliceSentence(string target, int length, string delimiter = "/")
         |   Get sliced string block
         |
         +- .SliceReverseSentence(string target, int length, string delimiter = "/")
         |   Get cutted string block
         |
         +- .Split(string target, string delimiter = " ")
         |   Get Splitted String
         |
         +- .IsAscii(string value)
         |   Validate string has only Single-Byte charactors
         |
         +- .IsMultiByte(string value, System.Text.Encoding encode = null)
         |   Validate string only Multi-Byte charactors
         |
         +- .GetLinefeed(Xb.Str.LinefeedType linefeed = Xb.Str.LinefeedType.CrLf)
         |   Get Linefeed charactor
         |
         +- .GetMultiLine(string multiLineText)
         |   Get Linefeed-Splitted multiline strings
         |
         +- .GetBytes(string target, System.Text.Encoding encode = null)
         |   Get Byte-Array from string
         |
         +- .GetByteLength(string target, System.Text.Encoding encode = null)
         |   Get Byte-Length from string
         |
         +- .EscapeHtml(string html)
         |   Escape Html-Special-Charactors
         |
         +- .UnescapeHtml(string html)
         |   Unescape Html-Special-Charactors
         |
         +- .MySqlQuote(string text)
         |   Quote string value, and Escape for MySql
         |
         +- .SqlQuote(string text)
         |   Quote string value, and Escape for Microsoft Sql Server
         |
         +- .Dquote(string text)
         |   Double-Quote string value, and Escape for JSON
         |
         +- .CsvDquote(string text)
         |   Double-Quote string value, and Escape for CSV
         |
         +- .GetString(byte[] bytes)
         |   Get string from Byte-Array, auto detect Japanese-Encode
         |
         +- .GetString(System.IO.Stream stream)
         |   Get string from Stream, auto detect Japanese-Encode
         |
         +- .GetEncode(byte[] bytes, bool forceJapaneseDetection = false)
         |   Detect Encode from Byte-Array(for Japanese)
         |
         +- .GetEncode(System.IO.Stream stream, bool forceJapaneseDetection = false)
             Detect Encode from Byte-Array(for Japanese)

    ・Xb.Util
          |
          +- .Out(string message)
          |   Output message to console
          |
          +- .Out(string format, params object[] values)
          |   Output formatted-message to console
          |
          +- .Out(Exception ex)
          |   Output Exception-Info to console
          |
          +- .OutHighlighted(params System.String[] messages)
          |   Output Highlighted message
          |
          +- .GetErrorString(Exception ex)
              Get Formatted Exception-Info string-array

## Contribution
1. Fork it ( https://github.com/ume05rw/Xb.Core/fork )
2. Create your feature branch (git checkout -b my-new-feature)
3. Commit your changes (git commit -am 'Add some feature')
4. Push to the branch (git push origin my-new-feature)
5. Create new Pull Request


## Licence

[MIT Licence](https://github.com/ume05rw/Xb.Core/blob/master/LICENSE)

## Author

[Do-Be's](http://dobes.jp)
