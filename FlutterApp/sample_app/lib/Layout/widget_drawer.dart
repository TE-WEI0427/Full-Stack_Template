import 'package:flutter/material.dart';

Widget drawer1(String accountName, String accountEmail) {
  return Drawer(
    child: ListView(
      padding: const EdgeInsets.only(),
      children: [
        UserAccountsDrawerHeader(
            accountName: Text(accountName),
            accountEmail: Text(accountEmail),
            currentAccountPicture: CircleAvatar(
                child: Text(accountName.substring(0, 1),
                    style: const TextStyle(fontSize: 40)))),
        ListTile(
          title: const Text("首頁"),
          leading: const Icon(Icons.home),
          onTap: () {
            debugPrint("首頁");
          },
        ),
        ListTile(
          title: const Text("統計"),
          leading: const Icon(Icons.business),
          onTap: () {
            debugPrint("統計");
          },
        ),
        ListTile(
          title: const Text("收藏"),
          leading: const Icon(Icons.favorite),
          onTap: () {
            debugPrint("收藏");
          },
        ),
        ListTile(
          title: const Text("紀錄"),
          leading: const Icon(Icons.collections_bookmark),
          onTap: () {
            debugPrint("紀錄");
          },
        ),
        ListTile(
          title: const Text("我的"),
          leading: const Icon(Icons.person),
          onTap: () {
            debugPrint("我的");
          },
        ),
      ],
    ),
  );
}
