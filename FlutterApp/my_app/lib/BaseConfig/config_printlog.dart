import 'dart:developer' as developer;

void devLog(String name, String message) {
  developer.log(message, name: name);
}

void devError(String name, Object error) {
  developer.log("", name: name, error: error);
}
