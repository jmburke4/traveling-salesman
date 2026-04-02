## dotnet test output
```
ban@DESKTOP-IICCLUK /cygdrive/r/traveling-salesman
$ dotnet test --logger "console;verbosity=detailed"
Restore complete (1.7s)
  Implementation net9.0 succeeded (4.6s) → Implementation\bin\Debug\net9.0\Implementation.dll
  Implementation.Tests net9.0 succeeded (1.7s) → Implementation.Tests\bin\Debug\net9.0\Implementation.Tests.dll
A total of 1 test files matched the specified pattern.
R:\traveling-salesman\Implementation.Tests\bin\Debug\net9.0\Implementation.Tests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 9.0.11)
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 9.0.11)
[xUnit.net 00:00:00.11]   Discovering: Implementation.Tests
[xUnit.net 00:00:00.11]   Discovering: Implementation.Tests
[xUnit.net 00:00:00.21]   Discovered:  Implementation.Tests
[xUnit.net 00:00:00.21]   Discovered:  Implementation.Tests
[xUnit.net 00:00:00.22]   Starting:    Implementation.Tests
[xUnit.net 00:00:00.22]   Starting:    Implementation.Tests
  Passed Implementation.Tests.AlgorithmTests.Algorithm_CalculateTourDistance_MatchesExpectedDistance [155 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(nodeCount: 10) [158 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(nodeCount: 6) [< 1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(nodeCount: 8) [< 1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_6NodeGraph_ReturnsValidTourAndConsistentDistance [< 1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_BestSolution_MatchesBruteForce(nodeCount: 8) [1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_BestSolution_MatchesBruteForce(nodeCount: 6) [< 1 ms]
  Passed Implementation.Tests.AlgorithmTests.Algorithm_ReadsDistanceMatrixFromFile_IntoSymmetricMatrix [25 ms]
  Passed Implementation.Tests.BruteForceTests.Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(nodeCount: 10) [431 ms]
  Passed Implementation.Tests.BruteForceTests.Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(nodeCount: 8) [2 ms]
  Passed Implementation.Tests.BruteForceTests.Solve_MultipleGraphSizes_ReturnsValidTourAndDistance(nodeCount: 6) [< 1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_BestSolution_MatchesBruteForce(nodeCount: 10) [283 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_SameMatrixRunTwice_ProducesSameDistance [< 1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_10NodeGraph_BeatsSequentialTourDistance [< 1 ms]
  Passed Implementation.Tests.GradientPotentialTests.Solve_8NodeUniformGraph_ReturnsExpectedTourDistance [< 1 ms]
  Passed Implementation.Tests.BruteForceTests.Solve_10NodeGraph_BeatsSequentialTourDistance [264 ms]
[xUnit.net 00:00:01.01]   Finished:    Implementation.Tests
[xUnit.net 00:00:01.01]   Finished:    Implementation.Tests
  Passed Implementation.Tests.BruteForceTests.Solve_8NodeUniformGraph_ReturnsExpectedTourDistance [3 ms]
  Passed Implementation.Tests.BruteForceTests.Solve_6NodeGraph_ReturnsValidTourAndConsistentDistance [< 1 ms]
  Passed Implementation.Tests.BruteForceTests.Solve_SameMatrixRunTwice_ProducesSameDistance [< 1 ms]

Test Run Successful.
Total tests: 19
     Passed: 19
 Total time: 1.8030 Seconds
  Implementation.Tests test net9.0 succeeded (2.3s)

Test summary: total: 19, failed: 0, succeeded: 19, skipped: 0, duration: 2.3s
Build succeeded in 10.7s

```

## graphs provided in class
```
ban@DESKTOP-IICCLUK /cygdrive/r/traveling-salesman
$ dotnet run --project Implementation/Implementation.csproj -- gp --file g100.graph --tour true
100%
Best distance: 1470516
Best tour: 0 -> 10 -> 20 -> 30 -> 31 -> 40 -> 50 -> 60 -> 70 -> 80 -> 90 -> 91 -> 81 -> 71 -> 41 -> 51 -> 61 -> 82 -> 72 -> 62 -> 92 -> 93 -> 83 -> 73 -> 63 -> 53 -> 54 -> 55 -> 65
 -> 64 -> 74 -> 94 -> 84 -> 95 -> 85 -> 75 -> 76 -> 86 -> 96 -> 97 -> 87 -> 66 -> 57 -> 67 -> 77 -> 88 -> 78 -> 47 -> 37 -> 58 -> 68 -> 79 -> 89 -> 98 -> 99 -> 69 -> 59 -> 49 -> 39
 -> 19 -> 9 -> 29 -> 48 -> 38 -> 18 -> 8 -> 28 -> 17 -> 7 -> 27 -> 16 -> 6 -> 5 -> 26 -> 36 -> 46 -> 56 -> 35 -> 45 -> 25 -> 24 -> 15 -> 4 -> 14 -> 3 -> 13 -> 23 -> 34 -> 44 -> 33
-> 43 -> 42 -> 52 -> 32 -> 22 -> 12 -> 11 -> 21 -> 1 -> 2 -> 0
Time: 0.015 seconds

ban@DESKTOP-IICCLUK /cygdrive/r/traveling-salesman
$ dotnet run --project Implementation/Implementation.csproj -- gp --file g250.graph --tour true
100%
Best distance: 3097
Best tour: 0 -> 1 -> 32 -> 16 -> 15 -> 31 -> 47 -> 48 -> 49 -> 64 -> 63 -> 80 -> 96 -> 79 -> 95 -> 111 -> 127 -> 128 -> 144 -> 129 -> 114 -> 112 -> 113 -> 97 -> 65 -> 66 -> 81 -> 9
8 -> 115 -> 99 -> 116 -> 131 -> 147 -> 132 -> 148 -> 164 -> 180 -> 181 -> 196 -> 179 -> 163 -> 146 -> 130 -> 145 -> 162 -> 178 -> 193 -> 177 -> 161 -> 176 -> 160 -> 143 -> 159 -> 1
75 -> 191 -> 192 -> 207 -> 223 -> 224 -> 209 -> 226 -> 211 -> 195 -> 227 -> 194 -> 210 -> 208 -> 239 -> 240 -> 241 -> 225 -> 242 -> 243 -> 212 -> 228 -> 244 -> 229 -> 245 -> 247 ->
 246 -> 230 -> 231 -> 215 -> 199 -> 214 -> 213 -> 197 -> 198 -> 182 -> 183 -> 166 -> 168 -> 135 -> 151 -> 134 -> 150 -> 165 -> 149 -> 133 -> 118 -> 119 -> 103 -> 120 -> 105 -> 137
-> 122 -> 138 -> 155 -> 154 -> 170 -> 153 -> 152 -> 136 -> 167 -> 200 -> 184 -> 185 -> 169 -> 202 -> 201 -> 217 -> 216 -> 232 -> 248 -> 249 -> 233 -> 218 -> 234 -> 235 -> 203 -> 18
7 -> 186 -> 171 -> 219 -> 236 -> 204 -> 220 -> 237 -> 221 -> 205 -> 188 -> 172 -> 189 -> 222 -> 238 -> 206 -> 190 -> 173 -> 174 -> 158 -> 157 -> 142 -> 110 -> 126 -> 141 -> 156 ->
139 -> 140 -> 125 -> 109 -> 108 -> 124 -> 123 -> 107 -> 92 -> 93 -> 94 -> 78 -> 62 -> 61 -> 77 -> 76 -> 60 -> 75 -> 59 -> 44 -> 45 -> 46 -> 30 -> 14 -> 29 -> 13 -> 28 -> 12 -> 27 -
> 11 -> 10 -> 9 -> 8 -> 24 -> 25 -> 41 -> 57 -> 26 -> 42 -> 43 -> 58 -> 74 -> 90 -> 91 -> 106 -> 121 -> 104 -> 88 -> 89 -> 73 -> 72 -> 71 -> 87 -> 86 -> 55 -> 56 -> 40 -> 39 -> 23
-> 7 -> 38 -> 54 -> 37 -> 22 -> 6 -> 21 -> 36 -> 53 -> 69 -> 70 -> 85 -> 68 -> 52 -> 83 -> 84 -> 117 -> 102 -> 101 -> 100 -> 67 -> 82 -> 51 -> 35 -> 19 -> 20 -> 5 -> 4 -> 3 -> 2 ->
 18 -> 34 -> 50 -> 33 -> 17 -> 0
Time: 0.098 seconds

ban@DESKTOP-IICCLUK /cygdrive/r/traveling-salesman
$ dotnet run --project Implementation/Implementation.csproj -- gp --file g1000.graph --tour true
100%
Best distance: 3567
Best tour: 0 -> 1 -> 65 -> 34 -> 66 -> 162 -> 98 -> 131 -> 99 -> 163 -> 67 -> 35 -> 5 -> 37 -> 7 -> 9 -> 8 -> 6 -> 38 -> 70 -> 4 -> 3 -> 100 -> 132 -> 164 -> 36 -> 68 -> 101 -> 197
 -> 229 -> 165 -> 69 -> 103 -> 134 -> 166 -> 230 -> 261 -> 294 -> 263 -> 262 -> 198 -> 133 -> 102 -> 71 -> 39 -> 40 -> 73 -> 135 -> 167 -> 199 -> 231 -> 232 -> 264 -> 296 -> 327 ->
 328 -> 360 -> 361 -> 394 -> 362 -> 330 -> 329 -> 297 -> 265 -> 200 -> 201 -> 168 -> 169 -> 138 -> 136 -> 104 -> 72 -> 41 -> 10 -> 11 -> 76 -> 107 -> 139 -> 108 -> 44 -> 77 -> 109
-> 142 -> 110 -> 78 -> 47 -> 46 -> 14 -> 45 -> 13 -> 12 -> 43 -> 75 -> 74 -> 42 -> 105 -> 137 -> 106 -> 170 -> 202 -> 233 -> 234 -> 266 -> 298 -> 299 -> 331 -> 332 -> 267 -> 235 ->
 203 -> 171 -> 140 -> 172 -> 173 -> 141 -> 174 -> 205 -> 204 -> 268 -> 269 -> 301 -> 270 -> 238 -> 237 -> 236 -> 300 -> 363 -> 396 -> 395 -> 364 -> 333 -> 365 -> 397 -> 366 -> 334
-> 302 -> 303 -> 271 -> 336 -> 305 -> 304 -> 335 -> 367 -> 399 -> 400 -> 368 -> 433 -> 496 -> 497 -> 464 -> 465 -> 401 -> 369 -> 337 -> 402 -> 370 -> 434 -> 403 -> 435 -> 468 -> 50
0 -> 436 -> 404 -> 372 -> 371 -> 341 -> 342 -> 246 -> 214 -> 182 -> 181 -> 245 -> 244 -> 276 -> 308 -> 339 -> 338 -> 307 -> 306 -> 275 -> 273 -> 274 -> 243 -> 211 -> 178 -> 242 ->
210 -> 241 -> 177 -> 209 -> 208 -> 240 -> 272 -> 239 -> 207 -> 206 -> 175 -> 176 -> 144 -> 111 -> 143 -> 112 -> 81 -> 80 -> 79 -> 48 -> 15 -> 16 -> 17 -> 49 -> 82 -> 113 -> 145 ->
114 -> 83 -> 51 -> 18 -> 50 -> 19 -> 52 -> 116 -> 84 -> 115 -> 148 -> 146 -> 179 -> 147 -> 212 -> 213 -> 180 -> 149 -> 117 -> 118 -> 150 -> 151 -> 86 -> 85 -> 22 -> 20 -> 53 -> 21
-> 54 -> 23 -> 55 -> 87 -> 119 -> 120 -> 56 -> 24 -> 88 -> 152 -> 153 -> 57 -> 25 -> 89 -> 122 -> 121 -> 154 -> 187 -> 186 -> 251 -> 250 -> 218 -> 185 -> 184 -> 183 -> 216 -> 217 -
> 249 -> 248 -> 215 -> 247 -> 279 -> 311 -> 310 -> 278 -> 277 -> 309 -> 340 -> 405 -> 373 -> 374 -> 343 -> 407 -> 406 -> 438 -> 471 -> 503 -> 535 -> 504 -> 537 -> 506 -> 473 -> 474
 -> 441 -> 442 -> 410 -> 411 -> 378 -> 379 -> 347 -> 315 -> 346 -> 345 -> 377 -> 376 -> 344 -> 312 -> 281 -> 280 -> 313 -> 282 -> 314 -> 316 -> 283 -> 284 -> 252 -> 219 -> 220 -> 1
55 -> 188 -> 156 -> 123 -> 124 -> 125 -> 93 -> 92 -> 91 -> 59 -> 90 -> 58 -> 26 -> 27 -> 60 -> 28 -> 29 -> 61 -> 157 -> 126 -> 62 -> 30 -> 94 -> 158 -> 189 -> 190 -> 221 -> 222 ->
254 -> 253 -> 285 -> 286 -> 317 -> 318 -> 350 -> 348 -> 349 -> 381 -> 382 -> 414 -> 413 -> 380 -> 443 -> 475 -> 476 -> 412 -> 445 -> 444 -> 478 -> 446 -> 510 -> 542 -> 606 -> 638 -
> 605 -> 574 -> 541 -> 477 -> 509 -> 507 -> 508 -> 539 -> 571 -> 602 -> 635 -> 603 -> 540 -> 572 -> 573 -> 604 -> 636 -> 637 -> 670 -> 669 -> 668 -> 732 -> 733 -> 765 -> 701 -> 702
 -> 734 -> 766 -> 797 -> 798 -> 830 -> 862 -> 894 -> 926 -> 925 -> 958 -> 990 -> 989 -> 957 -> 893 -> 861 -> 829 -> 796 -> 828 -> 892 -> 956 -> 988 -> 987 -> 986 -> 921 -> 953 -> 9
19 -> 951 -> 983 -> 982 -> 984 -> 920 -> 952 -> 985 -> 954 -> 955 -> 922 -> 890 -> 923 -> 924 -> 860 -> 827 -> 891 -> 859 -> 858 -> 826 -> 795 -> 764 -> 763 -> 762 -> 731 -> 700 ->
 667 -> 666 -> 698 -> 634 -> 601 -> 570 -> 569 -> 538 -> 536 -> 568 -> 600 -> 633 -> 665 -> 697 -> 730 -> 699 -> 794 -> 761 -> 793 -> 825 -> 857 -> 889 -> 888 -> 824 -> 856 -> 855
-> 887 -> 854 -> 822 -> 823 -> 792 -> 760 -> 791 -> 759 -> 728 -> 729 -> 696 -> 727 -> 664 -> 632 -> 663 -> 631 -> 630 -> 565 -> 534 -> 566 -> 599 -> 567 -> 505 -> 409 -> 408 -> 37
5 -> 440 -> 439 -> 472 -> 502 -> 470 -> 469 -> 437 -> 501 -> 533 -> 499 -> 467 -> 498 -> 466 -> 529 -> 530 -> 562 -> 594 -> 563 -> 531 -> 532 -> 597 -> 628 -> 629 -> 661 -> 598 ->
662 -> 694 -> 758 -> 695 -> 726 -> 790 -> 789 -> 820 -> 853 -> 852 -> 851 -> 883 -> 915 -> 884 -> 885 -> 886 -> 918 -> 950 -> 917 -> 949 -> 981 -> 948 -> 916 -> 980 -> 979 -> 947 -
> 978 -> 977 -> 944 -> 943 -> 974 -> 975 -> 976 -> 913 -> 914 -> 946 -> 945 -> 849 -> 882 -> 850 -> 818 -> 754 -> 819 -> 788 -> 821 -> 757 -> 725 -> 787 -> 756 -> 693 -> 724 -> 692
 -> 755 -> 723 -> 691 -> 658 -> 659 -> 660 -> 596 -> 564 -> 627 -> 595 -> 626 -> 561 -> 593 -> 625 -> 624 -> 560 -> 528 -> 559 -> 527 -> 495 -> 463 -> 431 -> 432 -> 398 -> 430 -> 4
62 -> 494 -> 526 -> 525 -> 493 -> 492 -> 460 -> 429 -> 461 -> 428 -> 427 -> 459 -> 491 -> 490 -> 522 -> 523 -> 587 -> 555 -> 524 -> 556 -> 557 -> 589 -> 588 -> 620 -> 619 -> 651 ->
 652 -> 684 -> 653 -> 654 -> 685 -> 717 -> 716 -> 683 -> 748 -> 780 -> 779 -> 749 -> 781 -> 813 -> 814 -> 782 -> 815 -> 783 -> 751 -> 750 -> 718 -> 686 -> 655 -> 623 -> 622 -> 621
-> 558 -> 590 -> 591 -> 592 -> 656 -> 687 -> 719 -> 720 -> 688 -> 689 -> 657 -> 690 -> 721 -> 752 -> 753 -> 722 -> 786 -> 817 -> 785 -> 784 -> 816 -> 881 -> 880 -> 848 -> 847 -> 87
9 -> 912 -> 911 -> 942 -> 878 -> 846 -> 877 -> 910 -> 973 -> 909 -> 941 -> 845 -> 844 -> 812 -> 811 -> 843 -> 876 -> 908 -> 940 -> 972 -> 971 -> 939 -> 907 -> 842 -> 810 -> 875 ->
938 -> 970 -> 937 -> 969 -> 905 -> 936 -> 904 -> 967 -> 935 -> 903 -> 968 -> 999 -> 966 -> 998 -> 965 -> 934 -> 933 -> 901 -> 902 -> 870 -> 871 -> 838 -> 869 -> 837 -> 868 -> 900 -
> 932 -> 997 -> 996 -> 964 -> 963 -> 931 -> 929 -> 865 -> 834 -> 899 -> 835 -> 802 -> 770 -> 771 -> 739 -> 740 -> 707 -> 674 -> 676 -> 708 -> 675 -> 642 -> 611 -> 643 -> 644 -> 677
 -> 645 -> 710 -> 742 -> 741 -> 709 -> 678 -> 646 -> 613 -> 612 -> 548 -> 515 -> 482 -> 483 -> 451 -> 418 -> 450 -> 449 -> 448 -> 416 -> 417 -> 385 -> 386 -> 387 -> 419 -> 420 -> 3
88 -> 355 -> 356 -> 358 -> 357 -> 323 -> 290 -> 289 -> 256 -> 258 -> 224 -> 225 -> 192 -> 194 -> 159 -> 127 -> 95 -> 32 -> 2 -> 33 -> 64 -> 96 -> 128 -> 130 -> 160 -> 226 -> 228 ->
 196 -> 227 -> 260 -> 259 -> 292 -> 293 -> 291 -> 324 -> 325 -> 326 -> 359 -> 295 -> 391 -> 392 -> 393 -> 426 -> 458 -> 489 -> 457 -> 456 -> 425 -> 424 -> 390 -> 389 -> 421 -> 485
-> 452 -> 484 -> 516 -> 517 -> 453 -> 454 -> 422 -> 423 -> 455 -> 488 -> 487 -> 486 -> 518 -> 550 -> 549 -> 582 -> 581 -> 614 -> 615 -> 583 -> 551 -> 519 -> 552 -> 520 -> 553 -> 52
1 -> 554 -> 586 -> 618 -> 650 -> 617 -> 585 -> 584 -> 616 -> 649 -> 648 -> 680 -> 647 -> 679 -> 712 -> 744 -> 745 -> 777 -> 713 -> 681 -> 714 -> 682 -> 715 -> 746 -> 747 -> 778 ->
874 -> 906 -> 873 -> 872 -> 840 -> 841 -> 809 -> 808 -> 776 -> 711 -> 743 -> 775 -> 839 -> 806 -> 807 -> 774 -> 773 -> 805 -> 804 -> 772 -> 803 -> 836 -> 867 -> 898 -> 930 -> 928 -
> 962 -> 995 -> 961 -> 992 -> 994 -> 993 -> 991 -> 960 -> 959 -> 927 -> 895 -> 896 -> 897 -> 832 -> 864 -> 863 -> 831 -> 800 -> 833 -> 866 -> 801 -> 768 -> 736 -> 769 -> 737 -> 738
 -> 706 -> 705 -> 673 -> 641 -> 640 -> 672 -> 639 -> 704 -> 735 -> 799 -> 767 -> 703 -> 671 -> 607 -> 575 -> 608 -> 576 -> 609 -> 577 -> 578 -> 514 -> 546 -> 547 -> 580 -> 610 -> 5
79 -> 513 -> 545 -> 481 -> 544 -> 512 -> 480 -> 511 -> 543 -> 479 -> 447 -> 415 -> 384 -> 383 -> 351 -> 354 -> 353 -> 320 -> 322 -> 321 -> 352 -> 319 -> 288 -> 287 -> 255 -> 257 ->
 223 -> 191 -> 193 -> 195 -> 161 -> 129 -> 97 -> 63 -> 31 -> 0
Time: 4.965 seconds

```
