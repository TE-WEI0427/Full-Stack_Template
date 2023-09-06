import 'package:flutter/material.dart';

class ClsBottomBar1 extends StatelessWidget {
  final int currentPageIndex;
  final Function(int) onPageChange;
  final Color? indicatorColor;
  final List<Widget> destinations;

  const ClsBottomBar1(
      {super.key,
      required this.currentPageIndex,
      required this.onPageChange,
      required this.indicatorColor,
      required this.destinations});

  @override
  Widget build(BuildContext context) {
    return NavigationBar(
      selectedIndex: currentPageIndex,
      onDestinationSelected: onPageChange,
      indicatorColor: indicatorColor,
      destinations: destinations,
    );
  }
}
