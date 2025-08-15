# 날씨기반 음악추천앱 DJ-Zeus

## [다운로드 APK (Latest)](https://github.com/talCSHN/DJ-Zeus/releases/tag/v1.0.0)

## 프로젝트 개요
- 현재 날씨에 어울리는 음악을 실시간으로 추천해주는 .NET MAUI 기반 풀스택 모바일 애플리케이션
- MAUI로 클라이언트, ASP.NET Core로 백엔드 API를 구축하여 Linux 환경의 개인 서버(라즈베리파이)에 배포

## 주요 기능
- 실시간 위치/날씨 정보: OpenWeatherMap API를 연동하여 현재 위치의 날씨 정보 실시간 조회

- 날씨 기반 음악 추천: 맑음, 비, 눈 등 날씨와 시간대에 따라 최적의 음악 플레이리스트 동적 추천

- 음악 검색 및 스트리밍: YouTube API로 음악을 검색하고, 앱 내에서 음악 스트리밍

- 실제 사용: .apk파일 설치 후 실행

## 기술 스택
**Client**
- .NET MAUI(C#)

**Server**
- ASP.NET Core(C#)

**Database**
- MariaDB

**배포 환경**
- Debian 12 Linux(Raspberry Pi 5)

**주요 API 및 라이브러리**
- Entity Framework Core
- CommunityToolkit.Mvvm
- OpenWeatherMap API
- YouTube Data API v3
- YoutubeExplode

## 아키텍처
클라이언트, 서버, 데이터베이스, 외부 API가 통신하는 풀스택 구조

- 클라이언트(.NET MAUI): 사용자의 위치 정보를 가져와 서버에 API 요청 전송

- 서버(ASP.NET Core): 라즈베리파이에서 실행되며, 클라이언트의 요청을 받아 외부 API와 통신하고, 추천 기록을 MariaDB에 저장한 뒤 최종 결과를 클라이언트에 전송

- 데이터베이스(MariaDB): 응답 데이터 저장
