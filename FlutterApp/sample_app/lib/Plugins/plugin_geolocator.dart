import 'package:geolocator/geolocator.dart';

/// 確定裝置的目前位置
///
/// 當位置服務未啟用或權限被拒絕時，「Future」將傳回錯誤。
Future<Position> determinePosition() async {
  bool serviceEnabled;
  LocationPermission permission;

  // 測試是否啟用定位服務
  serviceEnabled = await Geolocator.isLocationServiceEnabled();
  if (!serviceEnabled) {
    // 位置服務未啟用，請勿繼續訪問位置並請求用戶應用程式啟用位置服務。
    return Future.error('Location services are disabled.');
  }

  permission = await Geolocator.checkPermission();
  if (permission == LocationPermission.denied) {
    permission = await Geolocator.requestPermission();
    if (permission == LocationPermission.denied) {
      // 權限被拒絕，下次可以嘗試再次要求權限
      // 這也是 Android 的 shouldShowRequestPermissionRationale 返回 true 的地方
      // 根據 Android 指南，應用程式現在應該顯示解釋性 UI
      return Future.error('Location permissions are denied');
    }
  }

  if (permission == LocationPermission.deniedForever) {
    // 權限永久拒絕，請妥善處理。
    return Future.error(
        'Location permissions are permanently denied, we cannot request permissions.');
  }

  // 當我們到達這裡時，權限被授予，我們可以繼續訪問設備的位置。
  return await Geolocator.getCurrentPosition();
}
