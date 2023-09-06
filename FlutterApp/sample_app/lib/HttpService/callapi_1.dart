import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

/// Call API to Get Token
Future<void> getToken() async {
  Map actRow = {"Account": "User1", "Password": "admin"};

  var url = Uri.parse("http://10.0.2.2:5122/api/AllDemo/Login");

  String body = json.encode(actRow);

  try {
    var response = await http.post(url,
        headers: {"Content-Type": "application/json"}, body: body);

    if (response.statusCode == 201 || response.statusCode == 200) {
      var res = jsonDecode(utf8.decode(response.bodyBytes));
      if (res["resultCode"] == 10) {
        var data = res["data"];
        debugPrint(data["jwtToken"]);
      } else {
        debugPrint(res["message"]);
      }
    }
    // } on SocketException {
    //   debugPrint("Connection timed out");
  } catch (e) {
    if (e is SocketException) {
      //treat SocketException
      debugPrint("Socket exception: ${e.message.toString()}");
    } else if (e is TimeoutException) {
      //treat TimeoutException
      debugPrint("Timeout exception: ${e.toString()}");
    } else {
      debugPrint("Unhandled exception: ${e.toString()}");
    }
  }
}
