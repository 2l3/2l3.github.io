<?php

namespace App\Jobs;

use App\Models\Link;
use Exception;
use Illuminate\Bus\Queueable;
use Illuminate\Contracts\Queue\ShouldQueue;
use Illuminate\Foundation\Bus\Dispatchable;
use Illuminate\Queue\InteractsWithQueue;
use Illuminate\Queue\SerializesModels;

class LinkMetaData implements ShouldQueue
{
    use Dispatchable, InteractsWithQueue, Queueable, SerializesModels;

    /**
     * @var Link
     */
    private $link;

    /**
     * Create a new job instance.
     *
     * @param  Link  $link
     */
    public function __construct(Link $link)
    {
        $this->link = $link;
    }

    /**
     * Execute the job.
     *
     * @return void
     */
    public function handle()
    {
        try {
            $title = preg_match('/<title[^>]*>(.*?)<\/title>/ims', file_get_contents($this->link->url), $matches) ? $matches[1] : null;
            $metaTags = get_meta_tags($this->link->url);
            if (empty($this->link->title)) {
                $this->link->title = $title;
            }
            if (empty(json_decode($this->link->tags))) {
                if (isset($metaTags['keywords'])) {
                    $this->link->tags = explode(',', str_replace(', ', ',', $metaTags['keywords']));
                }
            }
            if (empty($this->link->description)) {
                if (isset($metaTags['description'])) {
                    $this->link->description = substr($metaTags['description'], 0, 250);
                }
            }
            $this->link->save();
        } catch (Exception $e) {
            report($e);
        }
    }
}
