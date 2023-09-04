import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

Future<void> GetToken() async {
  Map actRow = {"Account": "User1", "Password": "admin"};

  var url = Uri.parse("http://10.0.2.2:5122/api/AllDemo/Login");

  String body = json.encode(actRow);

  var response = await http.post(url,
      headers: {"Content-Type": "application/json"}, body: body);

  try {
    if (response.statusCode == 201 || response.statusCode == 200) {
      var res = jsonDecode(utf8.decode(response.bodyBytes));
      if (res["resultCode"] == 10) {
        var data = res["data"];
        debugPrint(data["jwtToken"]);
      } else {
        debugPrint(res["message"]);
      }
    }
  } catch (error) {
    debugPrint(error as String?);
  }
}
