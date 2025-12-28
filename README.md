# Rubik's Cube Solver

C#で実装されたルービックキューブのソルバーです。IDA*アルゴリズムと枝刈りテーブル（Pruning Tables）を使用して解を探索します。

## 必要条件

* .NET 10.0 SDK

## セットアップ

プログラムを実行する前に、必要なテーブルデータを生成して `data` ディレクトリに配置する必要があります。
プロジェクトのルートディレクトリに `data` フォルダを作成し、以下の手順に従ってください。

### 1. Move Table の生成

以下の各プロジェクトを実行し、生成された JSON ファイルを `data` ディレクトリに移動してください。

* `make_cp_move_table` -> `cp_move_table.json`
* `make_co_move_table` -> `co_move_table.json`
* `make_eo_move_table` -> `eo_move_table.json`

コマンド例:
```bash
dotnet run --project make_cp_move_table
mv cp_move_table.json data/
# 他も同様に実行
```

### 2. Prune Table の生成

Move Table が `data` ディレクトリにある状態で、以下のプロジェクトを実行してください。生成された JSON ファイルを `data` ディレクトリに移動します。

* `make_cp_co_prune_table` -> `cp_co_prune_table.json`
* `make_cp_eo_prune_table` -> `cp_eo_prune_table.json`
* `make_co_eo_prune_table` -> `co_eo_prune_table.json`

### 3. EP データの事前計算

以下のプロジェクトを実行し、生成された `ep_index_*.json` ファイル（`ep_index_6.json` ～ `ep_index_12.json`）をすべて `data` ディレクトリに移動します。
※この処理には時間がかかる場合があります。

* `pretrain_ep`

### 4. EP 辞書の生成

以下のプロジェクトを実行し、生成された `ep_dict.json` を `data` ディレクトリに移動します。

* `make_ep_dict`

## 実行方法

すべてのデータファイルが `data` ディレクトリに配置されたら、`cube` プロジェクトを実行してソルバーを起動します。

```bash
dotnet run --project cube
```

### スクランブルの変更

現在の実装では、スクランブル（キューブの初期状態）は `cube/Program.cs` 内にハードコードされています。
別のスクランブルを解く場合は、`Program.cs` の `scramble` 変数を編集して再ビルドしてください。

```csharp
scramble = "R' U' F R' B' F2 L2 D' U' L2 F2 D' L2 D' R B D2 L D2 F2 U2 L R' U' F";
```
