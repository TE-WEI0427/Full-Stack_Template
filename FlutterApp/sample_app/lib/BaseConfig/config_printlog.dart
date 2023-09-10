import 'dart:developer' as developer;

// 開發時，需要打印一些資料到偵錯主控台上，可使用這些方法

/// 打印 log (黃色)
///
/// ------------------------------------------------
/// 參數說明
///
/// name : 訊息 Title
///
/// message : 訊息內容
void devLog(String name, String message) {
  developer.log(message, name: name);
}

/// 打印 log (紅色)
///
/// ---------------------------
/// 參數說明
///
/// name : 訊息 Title
///
/// error : 訊息內容，可以是物件
void devError(String name, Object error) {
  developer.log("", name: name, error: error);
}
